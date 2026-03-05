namespace NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions.ReturnValue;

public class ActionWithTargetsAvailability : ActionAvailabilityBase
{
    public List<TargetSelectionPrompt> TargetSelectionPrompts { get; set; } = [];
}