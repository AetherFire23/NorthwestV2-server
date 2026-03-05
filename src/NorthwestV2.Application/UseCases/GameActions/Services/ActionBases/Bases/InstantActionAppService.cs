using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

/// <summary>
/// On the application layer, there's really no goal to this other than wiring everything together so the end-goal is
/// Just to return the Availabilityies. 
/// </summary>
public abstract class InstantActionAppService
{
    protected readonly NorthwestContext Context;

    // IDEA: Put player as property because he is executing the action. 
    protected InstantActionAppService(NorthwestContext context)
    {
        Context = context;
    }

    public abstract Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request);

    public abstract Task Execute(ExecuteActionRequest request);
}