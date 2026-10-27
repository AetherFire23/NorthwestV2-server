using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Plays a combat from its <see cref="CombatSetup"/> (docs/combat.md, "Order of a combat"). Pure and deterministic:
/// it does not touch the players, <see cref="CombatAction"/> applies the result.
///
/// Every decision a fighter takes during the combat (hit-and-run stop, closing in, push hard break-off) is made by
/// playing the alternatives ahead on a cloned <see cref="CombatState"/> and looking at how they end.
/// </summary>
public class CombatResolver
{
    public FightResult Resolve(CombatSetup setup)
    {
        CombatState state = CombatState.Start(setup);

        Play(state);

        return Finish(state);
    }

    private static void Play(CombatState state)
    {
        StrikeBySurprise(state);
        if (state.IsOver)
        {
            return;
        }

        if (state.Stance == AttackerStances.HitAndRun && WouldHitAndRunStop(state))
        {
            state.ExitType = FightExitType.StoppedByHitAndRun;
            return;
        }

        PlayFromClosingIn(state);
    }

    /// <summary>
    /// Step 1. One strike with no counter, and no stance modifier. Doubled in melee.
    /// </summary>
    private static void StrikeBySurprise(CombatState state)
    {
        if (!state.IsDefenderSurprised)
        {
            return;
        }

        Combatant attacker = state.Attacker;
        WeaponRanges range = attacker.HasRangedWeapon ? WeaponRanges.Ranged : WeaponRanges.Melee;

        decimal strength = attacker.Strength(range, 0);
        if (range == WeaponRanges.Melee)
        {
            strength *= CombatConstants.SURPRISE_MELEE_MULTIPLIER;
        }

        Strike(state, attacker, range, strength);
    }

    /// <summary>
    /// Checked once, after step 1, whether the defender was surprised or not: the attacker stops if the rest of the
    /// combat would take his Health to 40 or less. Otherwise the combat goes ahead in full.
    /// </summary>
    private static bool WouldHitAndRunStop(CombatState state)
    {
        CombatState lookAhead = state.Clone();
        PlayFromClosingIn(lookAhead);

        return lookAhead.Attacker.Health <= CombatConstants.HIT_AND_RUN_STOP_HEALTH;
    }

    private static void PlayFromClosingIn(CombatState state)
    {
        WeaponRanges roundsRange = CloseInIfWanted(state);
        if (state.IsOver)
        {
            return;
        }

        PlayRounds(state, roundsRange, canBreakOffUnderPushHard: true);
    }

    /// <summary>
    /// Step 2. Returns the range the rounds are fought at.
    /// - Melee attack: nobody is at a distance, so no closing in.
    /// - Defender without a ranged weapon: he always closes in.
    /// - Both armed at range: the attacker closes in if it gives him a better result than ranged rounds; if he does not,
    ///   the defender makes the same choice; if neither does, the rounds are ranged.
    /// </summary>
    private static WeaponRanges CloseInIfWanted(CombatState state)
    {
        if (!state.Attacker.HasRangedWeapon)
        {
            return WeaponRanges.Melee;
        }

        if (!state.Defender.HasRangedWeapon)
        {
            CloseIn(state, isAttackerClosing: false);
            return WeaponRanges.Melee;
        }

        if (IsClosingInBetter(state, isAttackerClosing: true))
        {
            CloseIn(state, isAttackerClosing: true);
            return WeaponRanges.Melee;
        }

        if (IsClosingInBetter(state, isAttackerClosing: false))
        {
            CloseIn(state, isAttackerClosing: false);
            return WeaponRanges.Melee;
        }

        return WeaponRanges.Ranged;
    }

    /// <summary>
    /// The fighter closing in takes one strike, with no counter, from the opponent's ranged weapon.
    /// </summary>
    private static void CloseIn(CombatState state, bool isAttackerClosing)
    {
        Combatant rangedHolder = isAttackerClosing ? state.Defender : state.Attacker;

        Strike(state, rangedHolder, WeaponRanges.Ranged, rangedHolder.Strength(WeaponRanges.Ranged, 0));
    }

    private static bool IsClosingInBetter(CombatState state, bool isAttackerClosing)
    {
        CombatState closingIn = state.Clone();
        CloseIn(closingIn, isAttackerClosing);
        if (!closingIn.IsOver)
        {
            PlayRounds(closingIn, WeaponRanges.Melee, canBreakOffUnderPushHard: true);
        }

        CombatState stayingAtRange = state.Clone();
        PlayRounds(stayingAtRange, WeaponRanges.Ranged, canBreakOffUnderPushHard: true);

        return IsBetterResult(closingIn, stayingAtRange, isAttackerClosing);
    }

    /// <summary>
    /// Winning beats not winning. Between two wins, more Health left is better; otherwise, leaving the opponent
    /// with less Health is better. A tie is not better, so the fighter keeps his distance.
    /// </summary>
    private static bool IsBetterResult(CombatState candidate, CombatState reference, bool fromAttackerSide)
    {
        (Combatant candidateMe, Combatant candidateOpponent) = Sides(candidate, fromAttackerSide);
        (Combatant referenceMe, Combatant referenceOpponent) = Sides(reference, fromAttackerSide);

        bool candidateWins = candidateOpponent.IsDown && !candidateMe.IsDown;
        bool referenceWins = referenceOpponent.IsDown && !referenceMe.IsDown;

        if (candidateWins != referenceWins)
        {
            return candidateWins;
        }

        return candidateWins
            ? candidateMe.Health > referenceMe.Health
            : candidateOpponent.Health < referenceOpponent.Health;
    }

    private static (Combatant Me, Combatant Opponent) Sides(CombatState state, bool fromAttackerSide)
    {
        return fromAttackerSide ? (state.Attacker, state.Defender) : (state.Defender, state.Attacker);
    }

    /// <summary>
    /// Step 3. The attacker strikes first, the defender strikes back if he is still up. The attacker breaks off when
    /// he has no Strength, or under push hard; the defender gets no free strike when he does.
    /// </summary>
    private static void PlayRounds(CombatState state, WeaponRanges range, bool canBreakOffUnderPushHard)
    {
        int stancePercent = CombatModifiers.StancePercent(state.Stance);

        for (int round = 0; round < CombatConstants.MAX_ROUNDS; round++)
        {
            decimal attackerStrength = state.Attacker.Strength(range, stancePercent);
            if (attackerStrength == 0)
            {
                state.ExitType = FightExitType.BrokeOffWithoutStrength;
                return;
            }

            Strike(state, state.Attacker, range, attackerStrength);
            if (state.IsOver)
            {
                return;
            }

            Strike(state, state.Defender, range, state.Defender.Strength(range, 0));
            if (state.IsOver)
            {
                return;
            }

            if (canBreakOffUnderPushHard && ShouldBreakOffUnderPushHard(state, range))
            {
                state.ExitType = FightExitType.BrokeOffUnderPushHard;
                return;
            }
        }

        throw new InvalidOperationException($"Combat did not end after {CombatConstants.MAX_ROUNDS} rounds.");
    }

    /// <summary>
    /// At the end of a round, at 20 Health or less, the push hard attacker plays the rest of the fight ahead without
    /// breaking off, and breaks off if he would go down first. A single strike from above 20 to 0 still kills him.
    /// </summary>
    private static bool ShouldBreakOffUnderPushHard(CombatState state, WeaponRanges range)
    {
        bool isLowOnHealth = state.Attacker.Health <= CombatConstants.PUSH_HARD_BREAK_OFF_HEALTH;
        if (state.Stance != AttackerStances.PushHard || !isLowOnHealth)
        {
            return false;
        }

        CombatState lookAhead = state.Clone();
        PlayRounds(lookAhead, range, canBreakOffUnderPushHard: false);

        return lookAhead.Attacker.IsDown;
    }

    private static void Strike(CombatState state, Combatant striker, WeaponRanges range, decimal strength)
    {
        Combatant target = state.OpponentOf(striker);
        Weapons weapon = striker.WeaponFor(range);

        if (!striker.WeaponsUsed.Contains(weapon))
        {
            striker.WeaponsUsed.Add(weapon);
        }

        target.ReceiveStrike(strength, striker.DamageDealt);

        if (target.IsDown)
        {
            state.ExitType = FightExitType.Knockout;
        }
    }

    /// <summary>
    /// Section 7: leftover Toughness damage is dropped (it only lives in <see cref="Combatant"/>), and 75% of the
    /// Health lost to stun damage comes back, rounded up. Status is decided before the refund: a fighter taken to 0 by
    /// stun damage stays stunned even with Health given back.
    /// </summary>
    private static FightResult Finish(CombatState state)
    {
        FightResult result = new FightResult
        {
            Attacker = state.Attacker,
            Defender = state.Defender,
            Stance = state.Stance,
            ExitType = state.ExitType ?? throw new InvalidOperationException("The combat ended without an exit type."),
            WasDefenderSurprised = state.IsDefenderSurprised,
            AttackerStatus = DetermineStatus(state.Attacker),
            DefenderStatus = DetermineStatus(state.Defender),
        };

        RefundStunDamage(state.Attacker);
        RefundStunDamage(state.Defender);

        return result;
    }

    private static PlayerLifeStatus DetermineStatus(Combatant combatant)
    {
        return combatant.DownedBy switch
        {
            null => PlayerLifeStatus.Alive,
            AttackTypes.Kill => PlayerLifeStatus.Dead,
            AttackTypes.Stun => PlayerLifeStatus.Stunned,
            _ => throw new ArgumentOutOfRangeException(nameof(combatant), combatant.DownedBy, null),
        };
    }

    private static void RefundStunDamage(Combatant combatant)
    {
        int refund = (int)Math.Ceiling(combatant.HealthLostToStun * CombatConstants.STUN_HEALTH_REFUND_RATIO);

        combatant.Health += refund;
    }
}
