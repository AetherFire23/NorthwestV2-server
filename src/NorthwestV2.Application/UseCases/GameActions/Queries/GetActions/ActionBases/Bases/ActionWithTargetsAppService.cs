using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ActionBases.Bases;

public abstract class ActionWithTargetsAppService
{
    protected readonly NorthwestContext Context;

    protected ActionWithTargetsAppService(NorthwestContext context)
    {
        Context = context;
    }

    public abstract ActionWithTargetsAvailability GetAvailabilityResult();
}