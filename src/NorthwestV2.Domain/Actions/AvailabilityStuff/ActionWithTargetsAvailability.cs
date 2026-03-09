using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

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