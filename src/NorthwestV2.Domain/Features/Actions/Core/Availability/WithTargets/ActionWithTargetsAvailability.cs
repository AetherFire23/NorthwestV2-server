using System.ComponentModel.DataAnnotations;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

/// <summary>
/// Allo am I seen 
/// </summary>
public class ActionWithTargetsAvailability : ActionAvailabilityBase
{
    
    /// <summary>
    /// It may be normal that a TagetSelectionPrompts is empty if some aciton requirement is invalid. 
    /// </summary>
    [Required]
    public List<TargetSelectionPrompt> TargetSelectionPrompts { get; set; } = [];
}