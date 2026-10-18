using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// One message about a fight and the players allowed to read it.
/// </summary>
public class CombatReport
{
    public required string Message { get; set; }
    public required List<Player> Recipients { get; set; }
}
