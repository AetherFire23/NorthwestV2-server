using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.Core.Application.Bases;

public abstract class ActionWithTargetsBase : ActionBase
{
    protected ActionWithTargetsBase(string actionName) : base(actionName)
    {
    }

    public abstract Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request);
}