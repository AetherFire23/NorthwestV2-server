using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsResult
{
    public required  List<InstantActionAvailability> InstantActions { get; set; } = [];
    public required List<ActionWithTargetsAvailability> ActionWithTargets { get; set; } = [];
}