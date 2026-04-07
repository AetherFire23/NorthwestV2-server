using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public class CombatAction
{
    public const int PLAYER_PROMPT_INDEX = 0;

    public const int ATTACKERSTANCE_PROMPT_INDEX = 1;

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

        return [targetSelection, selectAttackerStance];
    }

    /// <summary>
    /// Represents a single offensive action initiated by a player during combat.
    /// A <see cref="CombatAction"/> encapsulates:
    /// 
    /// • The attacker’s chosen <see cref="CombatStances"/> (HitAndRun, PushHard, ToTheEnd)  
    /// • The defender being targeted  
    /// • The temporary combat stats used for the resolution loop  
    /// • The resulting outcome (<see cref="FightResult"/>) including winner, loser, and exit type  
    /// 
    /// This class contains the full combat resolution flow:
    /// - Early‑exit checks (e.g., Hit‑and‑Run disengage)  
    /// - Iterative toughness exchanges and health damage  
    /// - Special stance logic (PushHard premature exit, ToTheEnd refusal to flee)  
    /// - Determination of death, victory, and post‑fight effects  
    /// </summary>
    public FightResult MakeTwoPlayerFightTogether(
        Player attackerPlayer,
        Player defenderPlayer, AttackerStances attackerStance)
    {
        /*
         * Extract the attacker's stance
         */


        // TODO: Ask waht "vigilance" means. I don't see anything anywhere. 
        // Build temporary combat stats for both players.
        // 'true' marks the attacker so stance logic so that Strength modifier can be calculated correctly. (ToTheEnd) 
        PlayerTempFightStats attackerStats = attackerPlayer.GetPlayerTempFightStats(true, attackerStance);
        PlayerTempFightStats defenderStats = defenderPlayer.GetPlayerTempFightStats(false, attackerStance);

        // Check if the attacker should immediately disengage due to Hit‑and‑Run rules.
        // If so, return the early‑exit result without entering the fight loop.
        if (MustEarlyExitBecauseOfHitAndRun(attackerStats, defenderStats, out FightResult earlyExitFightResult))
        {
            return earlyExitFightResult;
        }

        // Determine the initial fight state (usually Ongoing unless PushHard triggers early).
        FightExitType fightExitType = EvaluateIfMustExitLoopOrContinue(attackerStats, defenderStats);

        // Main combat loop: continues until a stance or death condition ends the fight.
        while (fightExitType == FightExitType.Ongoing)
        {
            // Both players simultaneously reduce each other's toughness by their Strength.
            defenderStats.CurrentToughness -= attackerStats.Strength;
            attackerStats.CurrentToughness -= defenderStats.Strength;

            // If toughness hits zero, convert that into health damage and reset toughness.
            DealDamageIfToughnessAtZero(attackerStats, defenderStats);
            DealDamageIfToughnessAtZero(defenderStats, attackerStats);

            // Re‑evaluate whether the fight should continue or exit.
            fightExitType = EvaluateIfMustExitLoopOrContinue(attackerStats, defenderStats);
        }

        // Special case: PushHard causes the attacker to flee when death is imminent.
        // In this scenario, the attacker always loses.
        if (fightExitType == FightExitType.ExittedBecauseOfPushHard)
        {
            return new FightResult
            {
                Winner = defenderPlayer,
                Loser = attackerPlayer,
                FightExitType = fightExitType,
            };
        }

        // If we reach this point, someone has died—no other exit conditions remain.
        // Determine winner/loser based on remaining health.
        Player winner = attackerStats.CurrentHealth >= 0
            ? attackerPlayer
            : defenderPlayer;

        Player loser = attackerStats.CurrentHealth >= 0
            ? defenderPlayer
            : attackerPlayer;

        // TODO: remember to apply death effect / create a domain Event 

        FightResult endFightResult = new FightResult()
        {
            Winner = winner,
            Loser = loser,
        };
        return endFightResult;
    }

    /// <summary>
    /// Determines whether the attacker should immediately disengage due to the
    /// <see cref="AttackerStances.HitAndRun"/> stance.
    /// 
    /// Hit‑and‑Run only happends when the attacker is weaker than the defender.
    /// If the conditions are met, the fight exits before any damage is exchanged.
    /// </summary>
    /// <param name="attackerStats">Temporary combat stats for the attacker.</param>
    /// <param name="defenderStats">Temporary combat stats for the defender.</param>
    /// <param name="fightResult">
    /// Output parameter containing the early‑exit <see cref="FightResult"/> 
    /// when Hit‑and‑Run triggers; otherwise an empty result.
    /// </param>
    /// <returns>
    /// <c>true</c> if the fight should end immediately due to Hit‑and‑Run; 
    /// otherwise <c>false</c>.
    /// </returns>
    private static bool MustEarlyExitBecauseOfHitAndRun(PlayerTempFightStats attackerStats,
        PlayerTempFightStats defenderStats,
        out FightResult fightResult)
    {
        // Attacker must have chosen explicitlykm.l,  Hit‑and‑Run.
        bool isHitAndRunStance = attackerStats.AttackerStance == AttackerStances.HitAndRun;

        // Hit‑and‑Run only works when the attacker is weaker than the defender.
        bool isDefenderStrengthTooHigh = attackerStats.Strength < defenderStats.Strength;

        // If both conditions are true, the attacker disengages before combat begins.
        if (isHitAndRunStance && isDefenderStrengthTooHigh)
        {
            fightResult = new FightResult
            {
                FightExitType = FightExitType.EarlyExittedBecauseOfHitAndRun,
            };

            return true;
        }

        // No early exit — return a default result and continue the fight.
        fightResult = new FightResult();
        return false;
    }

    /// <summary>
    /// If Toughness drops to 0, they lose Hleath and their toughness is restored to full
    /// </summary>
    private void DealDamageIfToughnessAtZero(PlayerTempFightStats player, PlayerTempFightStats defender)
    {
        if (player.CurrentToughness <= 0)
        {
            // Reset toughness
            player.CurrentToughness = player.BaseToughness;

            // Apply damage to health.
            player.CurrentHealth -= defender.Strength;
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
    /// <param name="attacker">The attacker's temporary combat stats.</param>
    /// <param name="defender">The defender's temporary combat stats.</param>
    /// <returns>
    /// A <see cref="FightExitType"/> describing whether the fight continues or ends.
    /// </returns>
    private FightExitType EvaluateIfMustExitLoopOrContinue(PlayerTempFightStats attacker, PlayerTempFightStats defender)
    {
        bool isAttackerStancePushHard = attacker.AttackerStance == AttackerStances.PushHard;
        bool attackerIsAboutToDie = attacker.CurrentHealth - defender.Strength <= 0;
        if (attackerIsAboutToDie && isAttackerStancePushHard)
        {
            return FightExitType.ExittedBecauseOfPushHard;
        }

        bool isPlayerDead = attacker.CurrentHealth <= 0 || defender.CurrentHealth < 0;

        if (isPlayerDead)
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
}