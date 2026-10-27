using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// How a combat ended. <see cref="Combatant.Health"/> is the final Health, stun refund included.
/// </summary>
public class FightResult
{
    public required Combatant Attacker { get; init; }
    public required Combatant Defender { get; init; }
    public required AttackerStances Stance { get; init; }
    public required FightExitType ExitType { get; init; }
    public required bool WasDefenderSurprised { get; init; }

    public required PlayerLifeStatus AttackerStatus { get; init; }
    public required PlayerLifeStatus DefenderStatus { get; init; }

    public bool IsStoppedByHitAndRun => ExitType == FightExitType.StoppedByHitAndRun;

    /// <summary>
    /// The fighter still up after a knockout. Null when the combat ended by the attacker leaving.
    /// </summary>
    public Combatant? Winner => ExitType != FightExitType.Knockout
        ? null
        : AttackerStatus != PlayerLifeStatus.Alive ? Defender : Attacker;

    public bool IsWinner(Player player)
    {
        return Winner is not null && Equals(Winner.Player, player);
    }
}
