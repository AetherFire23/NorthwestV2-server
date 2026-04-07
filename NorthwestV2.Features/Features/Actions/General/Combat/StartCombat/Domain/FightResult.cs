using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public class FightResult
{
    public FightExitType FightExitType { get; set; } = FightExitType.ExittedBecauseOfPlayerDeath;

    public Player Winner { get; set; }
    public Player Loser { get; set; }

    public bool IsWinner(Player player)
    {
        bool isWinner = Equals(this.Winner, player);

        return isWinner;
    }
}