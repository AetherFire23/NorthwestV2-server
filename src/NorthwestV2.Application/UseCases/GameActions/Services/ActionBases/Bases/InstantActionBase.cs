using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

/// <summary>
/// On the application layer, there's really no goal to this other than wiring everything together so the end-goal is
/// Just to return the Availabilityies. 
/// </summary>
public abstract class InstantActionBase : ActionBase
{
    protected readonly NorthwestContext Context;


    // IDEA: Put player as property because he is executing the action. 
    protected InstantActionBase(NorthwestContext context, string actionName) : base(context, actionName)
    {
        Context = context;
    }

    public abstract Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request);

    /// <summary>
    /// Verifying if an instant action can execute is vastly easier than with targets. 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> ValidateExecution(ExecuteActionRequest request)
    {
        InstantActionAvailability actionAvailability =
            await GetAvailabilityResult(new GetActionsRequest { PlayerId = request.PlayerId });

        return actionAvailability.CanExecute;
    }
}