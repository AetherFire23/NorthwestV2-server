using NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions;

public class GetActionsResult
{
    public List<InstantGameActionAvailability> InstantActions { get; set; } = [];
    public List<ActionWithTargetsAvailability> ActionWith { get; set; } = [];
}