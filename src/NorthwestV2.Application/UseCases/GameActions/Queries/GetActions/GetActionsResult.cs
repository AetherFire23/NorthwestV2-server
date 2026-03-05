using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsResult
{
    public required  List<InstantGameActionAvailability> InstantActions { get; set; } = [];
    public required List<ActionWithTargetsAvailability> ActionWithTargets { get; set; } = [];
}