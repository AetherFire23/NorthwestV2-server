using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

public abstract class ActionWithTargetsAppService
{
    protected readonly NorthwestContext Context;
    public string ActionName { get; set; }

    protected ActionWithTargetsAppService(NorthwestContext context, string actionName)
    {
        Context = context;
        ActionName = actionName;
    }

    public abstract Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request);
}