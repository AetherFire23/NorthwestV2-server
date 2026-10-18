using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

public class FightResult
{
    public FightExitType FightExitType { get; set; } = FightExitType.ExittedBecauseOfPlayerDeath;

    public PlayerTempFightStats AttackerFightStats { get; set; }
    public PlayerTempFightStats DefenderFightStats { get; set; }

    public PlayerLifeStatus AttackerStatus { get; set; } = PlayerLifeStatus.Alive;
    public PlayerLifeStatus DefenderStatus { get; set; } = PlayerLifeStatus.Alive;

    /// <summary>
    /// False when the attacker got away unseen (hit-and-run).
    /// </summary>
    public bool AttackerIdentified { get; set; } = true;

    /*
     * There may be no stricit *winner* depending on the exit type. Only Exitted Becausae of playr deaht causes a winner / loser.
     */
    /// <summary>
    /// The combatant still standing (Health above 0) when the fight ended by a knockout.
    /// Null when both went down at the same time, and for fights that ended by fleeing.
    /// </summary>
    public PlayerTempFightStats? SurvivingPlayer { get; set; }

    /// <summary>
    /// The combatant who ended up <see cref="PlayerLifeStatus.Dead"/>. Null when nobody died or both did.
    /// </summary>
    public PlayerTempFightStats? DeadPlayer
    {
        get
        {
            bool attackerDead = AttackerStatus == PlayerLifeStatus.Dead;
            bool defenderDead = DefenderStatus == PlayerLifeStatus.Dead;

            if (attackerDead == defenderDead)
            {
                return null;
            }

            return attackerDead ? AttackerFightStats : DefenderFightStats;
        }
    }

    public bool IsWinner(Player player)
    {
        bool isWinner = SurvivingPlayer is not null && Equals(this.SurvivingPlayer.Player, player);

        return isWinner;
    }
}
