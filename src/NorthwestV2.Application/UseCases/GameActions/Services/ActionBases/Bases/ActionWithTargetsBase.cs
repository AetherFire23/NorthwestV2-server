using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

public abstract class ActionWithTargetsBase : ActionBase
{
    protected readonly NorthwestContext Context;


    protected ActionWithTargetsBase(NorthwestContext context, string actionName) : base(context,actionName)
    {
        Context = context;
    }

    public abstract Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request);
}