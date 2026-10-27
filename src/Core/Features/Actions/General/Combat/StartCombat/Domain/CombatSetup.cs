using NorthwestV2.Features.Actions.Core.Domain;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Everything fixed before the combat starts.
/// </summary>
public class CombatSetup
{
    public required Combatant Attacker { get; init; }
    public required Combatant Defender { get; init; }
    public required AttackerStances Stance { get; init; }
    public required bool IsDefenderSurprised { get; init; }
}
