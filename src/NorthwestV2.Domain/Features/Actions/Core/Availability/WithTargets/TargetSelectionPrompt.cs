using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

/// <summary>
/// Represents a prompt instructing the user to select one or more valid action targets.
/// This model defines both the semantic description of the selection task and the
/// structural constraints governing how many targets may be chosen.
/// </summary>
public class TargetSelectionPrompt
{
    /// <summary>
    /// A human‑readable explanation of what the user is being asked to select.
    /// This should clearly communicate the intent of the selection (e.g., 
    /// "Choose the units you want to apply this action to.").
    /// </summary>
    [Required]
    public required string Description { get; set; }

    /// <summary>
    /// The complete set of targets the user is allowed to choose from.
    /// This list is authoritative: only these targets may be selected.
    /// Must contain at least one element.
    [Required]
    public required List<ActionTarget> ValidTargets { get; set; } = [];

    /// <summary>
    /// The minimum number of targets the user must select.
    /// Must be greater than or equal to 1 and less than or equal to <see cref="Max"/>.
    /// </summary>
    [Required]
    public int Min { get; set; } = 1;

    /// <summary>
    /// The maximum number of targets the user may select.
    /// Must be greater than or equal to <see cref="Min"/>.
    /// </summary>
    [Required]
    public int Max { get; set; } = 1;

    public override string ToString()
    {
        return this.Description;
    }
}