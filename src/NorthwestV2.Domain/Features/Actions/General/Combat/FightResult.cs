using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

public class FightResult
{
    public FightLoopStates FightLoopStates { get; set; } = FightLoopStates.ExittedBecauseOfPlayerDeath;

    public Player Winner { get; set; }
    public Player Loser { get; set; }

    public bool IsWinner(Player player)
    {
        bool isWinner = Equals(this.Winner, player);

        return isWinner;
    }
}