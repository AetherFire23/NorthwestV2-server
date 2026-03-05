using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

public class ActionWithTargetsAvailability : ActionAvailabilityBase
{
    /// <summary>
    /// It may be normal that a TagetSelectionPrompts is empty if some aciton requirement is invalid. 
    /// </summary>
    public List<TargetSelectionPrompt> TargetSelectionPrompts { get; set; } = [];
}