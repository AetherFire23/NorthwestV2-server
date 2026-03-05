using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

public class ActionWithTargetsAvailability : ActionAvailabilityBase
{
    public ActionWithTargetsAvailability(List<ActionRequirement> actionRequirements) : base(actionRequirements)
    {
    }

    /// <summary>
    /// It may be normal that a TagetSelectionPrompts is empty if some aciton requirement is invalid. 
    /// </summary>
    public List<TargetSelectionPrompt> TargetSelectionPrompts { get; set; } = [];
}