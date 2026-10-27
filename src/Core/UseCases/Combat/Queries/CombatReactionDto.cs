using System.ComponentModel.DataAnnotations;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.Combat.Queries;

/// <summary>
/// How the player fights back when this opponent attacks him with this attack type.
/// </summary>
public class CombatReactionDto
{
    [Required] public required Guid OpponentId { get; set; }
    [Required] public required Roles OpponentRole { get; set; }
    [Required] public required AttackTypes AttackType { get; set; }
    [Required] public required AttackTypes Counter { get; set; }

    /// <summary>
    /// Only used for a kill counter.
    /// </summary>
    [Required] public required WeaponPolicies WeaponPolicy { get; set; }

    /// <summary>
    /// Null for loyals, so the response never hints at an allegiance it does not concern.
    /// </summary>
    public FakedHopePenalties? FakedHopePenalty { get; set; }
}
