using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

public class CombatAction
{
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

        List<ActionRequirement> requiresAnyPlayerInTheSameRoom = DetermineRequirements(caster, otherPlayersInSameRoom);

        List<TargetSelectionPrompt> promptWithSingleScreenOfJustPlayers = CreatePrompts(otherPlayersInSameRoom);

        ActionWithTargetsAvailability availability = new ActionWithTargetsAvailability()
        {
            ActionName = ActionNames.CombatAction,
            TargetSelectionPrompts = promptWithSingleScreenOfJustPlayers,
            ActionRequirements = requiresAnyPlayerInTheSameRoom,
        };

        return availability;
    }

    private List<ActionRequirement> DetermineRequirements(Player caster, List<Player> otherPlayersInSameRoom)
    {
        ActionRequirement playersInSameRoomRequirement =
            ActionRequirement.CreatePlayerExistsInRoomRequirement(caster, otherPlayersInSameRoom);


        return [playersInSameRoomRequirement];
    }

    private List<TargetSelectionPrompt> CreatePrompts(List<Player> otherPlayersInSameRoom)
    {
        TargetSelectionPrompt targetSelection = TargetSelectionPrompt.FromPlayers(otherPlayersInSameRoom);

        return [targetSelection];
    }

    /*
     * Combat resolves in Rounds
     * For Each Round Both sides apply their strength.
     * Strength = Toughness + Weapon + modifiers
     *
     * (simultaneously)
     * player1.Toughness -= player2.Strength
     * player2.Toughness -= player1.Strength
     *
     * If player1.Toughness == 0|| player2.toughness == 0
     *
     * player1
     *
     */
    public FightResult MakeTwoPlayerFightTogether(Player attackerPlayer, Player defenderPlayer)
    {
        // TODO: Ask waht "vigilance" means. I don't see anything anywhere. 
        PlayerTempFightStats attackerStats = attackerPlayer.GetPlayerTempFightStats(true);
        PlayerTempFightStats defenderStats = defenderPlayer.GetPlayerTempFightStats(false);

        if (MustEarlyExit(attackerStats, defenderStats, out FightResult earlyExitFight))
        {
            return earlyExitFight;
        }

        // Condition may never be reached. For example if we try to attack with PushHard.

        int rounds = 0;
        FightLoopStates fightLoopStates = GetFightLoopState(attackerStats, defenderStats);
        while (fightLoopStates == FightLoopStates.Ongoing)
        {
            defenderStats.CurrentToughness -= attackerStats.Strength;
            attackerStats.CurrentToughness -= defenderStats.Strength;

            DealDamageIfToughnessAtZero(attackerStats, defenderStats);
            DealDamageIfToughnessAtZero(defenderStats, attackerStats);

            fightLoopStates = GetFightLoopState(attackerStats, defenderStats);
            rounds++;
        }

        if (fightLoopStates == FightLoopStates.ExittedBecauseOfPushHard)
        {
            // If PushHard, we know that the loser is the attacker
            return new FightResult()
            {
                Winner = defenderPlayer,
                Loser = attackerPlayer,
                FightLoopStates = fightLoopStates,
            };
        }

        // From here on now, a player was killed., because no more exit conditions are possible

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

    private static bool MustEarlyExit(PlayerTempFightStats attackerStats, PlayerTempFightStats defenderStats,
        out FightResult fightResult)
    {
        bool isHitAndRunStance = attackerStats.AttackerStance == AttackerStances.HitAndRun;
        bool isDefenderStrengthTooHigh = attackerStats.Strength < defenderStats.Strength;
        if (isHitAndRunStance && isDefenderStrengthTooHigh)
        {
            fightResult = new FightResult
            {
                FightLoopStates = FightLoopStates.EarlyExittedBecauseOfHitAndRun,
            };
        }

        // No assignment = must not exit
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
    /// Must exit a fight mid only when a player dies or the attacker is about to die with the "push hard" option
    /// enabled.  
    /// </summary>
    private FightLoopStates GetFightLoopState(PlayerTempFightStats attacker, PlayerTempFightStats defender)
    {
        
        bool isAttackerStancePushHard = attacker.AttackerStance == AttackerStances.PushHard;
        bool attackerIsAboutToDie = attacker.CurrentHealth - defender.Strength <= 0;
        if (attackerIsAboutToDie && isAttackerStancePushHard)
        {
            return FightLoopStates.ExittedBecauseOfPushHard;
        }
        
        bool isPlayerDead = attacker.CurrentHealth <= 0 || defender.CurrentHealth < 0;

        if (isPlayerDead)
        {
            return FightLoopStates.ExittedBecauseOfPlayerDeath;
        }



        return FightLoopStates.Ongoing;
    }
}