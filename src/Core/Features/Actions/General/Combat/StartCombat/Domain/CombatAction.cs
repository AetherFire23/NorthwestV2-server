using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Domain logic for combat resolution. Contains the fight loop, stance mechanics, and damage calculation.
/// Mutates the players it is given but does not save anything. Use <see cref="CombatActionApp"/> for persistence.
/// </summary>
public class CombatAction
{
    public const int PLAYER_PROMPT_INDEX = 0;

    public const int ATTACKERSTANCE_PROMPT_INDEX = 1;

    public const int ATTACKTYPE_PROMPT_INDEX = 2;

    /*
     * Attack types
     *  - Kill
     *  - Stun
     *
     * Defensive Counters ( Defined in another action
     *  - Default vs stun
     *  - Override - kill counter
     *
     * Stances
     *  - Hit and run -> Disengages immediately if strength gap is too low; attacker avoids damage and likely avoids identification.
     *  - Push Hard -> Attacker continues until losing inevitable, then flees, if no kill achieved, they are identified.
     *
     */
    public ActionWithTargetsAvailability DetermineAvailability(Player caster, List<Player> otherPlayersInSameRoom)
    {
        if (otherPlayersInSameRoom.Any(x => x.Id == caster.Id))
        {
            throw new Exception("The caster cannot target himself.");
        }

        /*
         * I don't know the real requirements yet....
         */
        List<ActionRequirement> requiresAnyPlayerInTheSameRoom = DetermineRequirements(caster, otherPlayersInSameRoom);

        List<TargetSelectionPrompt> promptWithSingleScreenOfJustPlayers = CreatePrompts(otherPlayersInSameRoom);

        ActionWithTargetsAvailability availability = new ActionWithTargetsAvailability()
        {
            ActionName = ActionNames.CombatAction,
            DisplayName = ActionNames.CombatAction,
            TargetSelectionPrompts = promptWithSingleScreenOfJustPlayers,
            ActionRequirements = requiresAnyPlayerInTheSameRoom,
        };

        return availability;
    }

    private List<ActionRequirement> DetermineRequirements(Player caster, List<Player> otherPlayersInSameRoom)
    {
        ActionRequirement playersInSameRoomRequirement =
            ActionRequirement.CreateAnyOtherPlayerExistsInRoomRequirement(caster, otherPlayersInSameRoom);

        return [playersInSameRoomRequirement];
    }

    private List<TargetSelectionPrompt> CreatePrompts(List<Player> otherPlayersInSameRoom)
    {
        TargetSelectionPrompt targetSelection = TargetSelectionPrompt.FromPlayers(otherPlayersInSameRoom);
        TargetSelectionPrompt selectAttackerStance = TargetSelectionPrompt.FromEnum<AttackerStances>(
            "Pick your attacker stance"
        );

        TargetSelectionPrompt selectAttackType = TargetSelectionPrompt.FromEnum<AttackTypes>(
            "Pick your attack type"
        );

        return [targetSelection, selectAttackerStance, selectAttackType];
    }

    /// <summary>
    /// Resolves a fight between two players and applies the outcome (Health and life status) to both entities.
    /// Persisting is up to the caller (<see cref="CombatActionApp"/>).
    ///
    /// Resolution:
    /// - Early exit when a hit-and-run attacker is weaker than the defender
    /// - Each round both sides apply their Strength to the other's toughness at the same time
    /// - A combatant whose toughness reaches 0 loses Health and gets its toughness back
    /// - PushHard attackers flee once the next blow would kill them; ToTheEnd attackers never flee
    /// - At 0 Health a combatant dies if the opponent was lethal, and is stunned otherwise
    /// - Allegiance is ignored on purpose: two acolytes may fight each other (see docs/traitors.md)
    /// </summary>
    public FightResult MakeTwoPlayerFightTogether(
        Player attackerPlayer,
        Player defenderPlayer, AttackerStances attackerStance)
    {
        // TODO: Ask waht "vigilance" means. I don't see anything anywhere.
        // 'true' marks the attacker so stance logic so that Strength modifier can be calculated correctly. (ToTheEnd)
        PlayerTempFightStats attackerStats = attackerPlayer.GetPlayerTempFightStats(true, attackerStance);
        PlayerTempFightStats defenderStats = defenderPlayer.GetPlayerTempFightStats(false, attackerStance);

        if (MustEarlyExitBecauseOfHitAndRun(attackerStats, defenderStats, out FightResult earlyExitFightResult))
        {
            earlyExitFightResult.AttackerFightStats = attackerStats;
            earlyExitFightResult.DefenderFightStats = defenderStats;
            earlyExitFightResult.AttackerIdentified = false;

            return earlyExitFightResult;
        }

        FightExitType fightExitType = EvaluateIfMustExitLoopOrContinue(attackerStats, defenderStats);

        while (fightExitType == FightExitType.Ongoing)
        {
            // Both players simultaneously reduce each other's toughness by their Strength.
            defenderStats.CurrentToughness -= attackerStats.Strength;
            attackerStats.CurrentToughness -= defenderStats.Strength;

            DealDamageIfToughnessAtZero(attackerStats, defenderStats);
            DealDamageIfToughnessAtZero(defenderStats, attackerStats);

            fightExitType = EvaluateIfMustExitLoopOrContinue(attackerStats, defenderStats);
        }

        FightResult fightResult = new FightResult
        {
            AttackerFightStats = attackerStats,
            DefenderFightStats = defenderStats,
            FightExitType = fightExitType,
        };

        if (fightExitType == FightExitType.ExittedBecauseOfPushHard)
        {
            // The attacker flees without finishing the defender, so they are identified.
            ApplyEndFightConditions(fightResult);
            return fightResult;
        }

        bool isAttackLethal = attackerStats.AttackType == AttackTypes.Kill;
        bool isCounterLethal = isAttackLethal || defenderStats.DefensiveCounter == DefensiveCounters.Override;

        fightResult.AttackerStatus = DetermineStatusAfterFight(attackerStats, isCounterLethal);
        fightResult.DefenderStatus = DetermineStatusAfterFight(defenderStats, isAttackLethal);
        fightResult.SurvivingPlayer = new[] { attackerStats, defenderStats }
            .SingleOrDefault(x => x.CurrentHealth > 0);

        ApplyEndFightConditions(fightResult);
        return fightResult;
    }

    /// <summary>
    /// A combatant at 0 Health is dead if the blows that took them down were lethal, and stunned otherwise.
    /// The attacker's blows are lethal with <see cref="AttackTypes.Kill"/>. The defender's blows are lethal when
    /// the attack is, or when they chose the <see cref="DefensiveCounters.Override"/> kill counter.
    /// </summary>
    private static PlayerLifeStatus DetermineStatusAfterFight(PlayerTempFightStats combatant,
        bool areOpponentBlowsLethal)
    {
        if (combatant.CurrentHealth > 0)
        {
            return PlayerLifeStatus.Alive;
        }

        return areOpponentBlowsLethal ? PlayerLifeStatus.Dead : PlayerLifeStatus.Stunned;
    }

    /// <summary>
    /// Determines whether the attacker should immediately disengage due to the
    /// <see cref="AttackerStances.HitAndRun"/> stance.
    ///
    /// Hit‑and‑Run only happends when the attacker is weaker than the defender.
    /// If the conditions are met, the fight exits before any damage is exchanged.
    /// </summary>
    private static bool MustEarlyExitBecauseOfHitAndRun(PlayerTempFightStats attackerStats,
        PlayerTempFightStats defenderStats,
        out FightResult fightResult)
    {
        bool isHitAndRunStance = attackerStats.AttackerStance == AttackerStances.HitAndRun;

        // Hit‑and‑Run only works when the attacker is weaker than the defender.
        bool isDefenderStrengthTooHigh = attackerStats.Strength < defenderStats.Strength;

        if (isHitAndRunStance && isDefenderStrengthTooHigh)
        {
            fightResult = new FightResult
            {
                FightExitType = FightExitType.EarlyExittedBecauseOfHitAndRun,
            };

            return true;
        }

        fightResult = new FightResult();
        return false;
    }

    /// <summary>
    /// If Toughness drops to 0, they lose Health and their toughness is restored to full
    /// </summary>
    private void DealDamageIfToughnessAtZero(PlayerTempFightStats player, PlayerTempFightStats opponent)
    {
        if (player.CurrentToughness <= 0)
        {
            player.CurrentToughness = player.BaseToughness;
            player.CurrentHealth -= opponent.Strength;
        }
    }

    /// <summary>
    /// Evaluates the current state of the fight loop and determines whether combat
    /// should continue or exit.
    ///
    /// Exit conditions:
    /// • <see cref="FightExitType.ExittedBecauseOfPushHard"/> — attacker is about to die while using PushHard
    /// • <see cref="FightExitType.ExittedBecauseOfPlayerDeath"/> — either combatant has reached zero (or below) health
    /// • <see cref="FightExitType.Ongoing"/> — no exit condition met; continue the loop
    /// </summary>
    private FightExitType EvaluateIfMustExitLoopOrContinue(PlayerTempFightStats attacker, PlayerTempFightStats defender)
    {
        bool isAttackerStancePushHard = attacker.AttackerStance == AttackerStances.PushHard;
        bool attackerIsAboutToDie = attacker.CurrentHealth - defender.Strength <= 0;
        if (attackerIsAboutToDie && isAttackerStancePushHard)
        {
            return FightExitType.ExittedBecauseOfPushHard;
        }

        bool isPlayerDown = attacker.CurrentHealth <= 0 || defender.CurrentHealth <= 0;

        if (isPlayerDown)
        {
            return FightExitType.ExittedBecauseOfPlayerDeath;
        }

        return FightExitType.Ongoing;
    }

    public Guid DetermineDefenderPlayer(List<List<ActionTarget>> requestActionTargets)
    {
        Guid attackerPlayerId = requestActionTargets[PLAYER_PROMPT_INDEX].First().TargetId ??
                                throw new Exception("target it cannot be null.");

        return attackerPlayerId;
    }

    public AttackerStances DetermineAttackerStance(List<List<ActionTarget>> requestActionTargets)
    {
        string attackerStanceValue = requestActionTargets[ATTACKERSTANCE_PROMPT_INDEX].First().Value ??
                                     throw new Exception("Must select an attackerstance ");

        AttackerStances attackerStance = Enum.Parse<AttackerStances>(attackerStanceValue);

        return attackerStance;
    }

    public AttackTypes DetermineAttackType(List<List<ActionTarget>> requestActionTargets)
    {
        string attackTypeValue = requestActionTargets[ATTACKTYPE_PROMPT_INDEX].First().Value ??
                                 throw new Exception("Must select an attack type ");

        AttackTypes attackType = Enum.Parse<AttackTypes>(attackTypeValue);

        return attackType;
    }

    /// <summary>
    /// Writes the outcome back to the players. Only Health and life status outlive the fight:
    /// toughness lives in <see cref="PlayerTempFightStats"/> and is therefore full again next fight.
    /// </summary>
    public void ApplyEndFightConditions(FightResult fightResult)
    {
        ApplyToPlayer(fightResult.AttackerFightStats, fightResult.AttackerStatus);
        ApplyToPlayer(fightResult.DefenderFightStats, fightResult.DefenderStatus);
    }

    private static void ApplyToPlayer(PlayerTempFightStats stats, PlayerLifeStatus status)
    {
        stats.Player.Health = Math.Max(0, stats.CurrentHealth);
        stats.Player.LifeStatus = status;
    }
}
