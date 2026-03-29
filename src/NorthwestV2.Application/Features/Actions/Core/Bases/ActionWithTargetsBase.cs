using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Core.Bases;

public abstract class ActionWithTargetsBase : ActionBase
{
    protected ActionWithTargetsBase(string actionName) : base(actionName)
    {
    }

    public abstract Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request);
}