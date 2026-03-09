using System.ComponentModel.DataAnnotations;

namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

public class TargetSelectionPrompt
{
    [Required] public required string Description { get; set; }
    [Required] public required List<ActionTarget> ValidTargets { get; set; } = [];
    [Required] public int Min { get; set; } = 1;
    [Required] public int Max { get; set; } = 1;
}