using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ActionBases.Bases;

/// <summary>
/// On the application layer, there's really no goal to this other than wiring everything together so the end-goal is
/// Just to return the Availabilityies. 
/// </summary>
public abstract class InstantActionAppService
{
    protected readonly NorthwestContext Context;

    protected InstantActionAppService(NorthwestContext context)
    {
        Context = context;
    }

    public abstract Task<InstantGameActionAvailability> GetAvailabilityResult(GetActionsRequest request);
}