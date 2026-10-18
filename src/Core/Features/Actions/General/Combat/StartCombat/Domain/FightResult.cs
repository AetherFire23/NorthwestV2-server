using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public class FightResult
{
    public FightExitType FightExitType { get; set; } = FightExitType.ExittedBecauseOfPlayerDeath;

    // public Player Winner { get; set; }
    // public Player Loser { get; set; }

    public PlayerTempFightStats AttackerFightStats { get; set; }
    public PlayerTempFightStats DefenderFightStats { get; set; }


    /*
     * There may be no stricit *winner* depending on the exit type. Only Exitted Becausae of playr deaht causes a winner / loser.
     */
    public PlayerTempFightStats? SurvivingPlayer { get; set; }

    public PlayerTempFightStats DeadPlayer =>
        AttackerFightStats == SurvivingPlayer ? AttackerFightStats : DefenderFightStats;


    public bool IsWinner(Player player)
    {
        bool isWinner = Equals(this.SurvivingPlayer.Player, player);

        return isWinner;
    }
}