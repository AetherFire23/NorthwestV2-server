using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsResult
{
    public required  List<InstantActionAvailability> InstantActions { get; set; } = [];
    public required List<ActionWithTargetsAvailability> ActionWithTargets { get; set; } = [];
}