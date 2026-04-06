using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Instant;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Core.Application.Bases;

/// <summary>
/// Base class for actions that execute immediately and do not require target selection.
/// </summary>
public abstract class InstantActionBase : ActionBase
{
    /// <summary>
    /// Initializes a new instant action with the given database context and action name.
    /// </summary>
    /// <param name="context">The database context used by the action.</param>
    /// <param name="actionName">The unique name of the action.</param>
    protected InstantActionBase(string actionName) : base(actionName)
    {
    }
    
    /// <summary>
    /// Computes whether the action is currently available to the requesting player,
    /// returning any relevant metadata needed by the UI.
    /// </summary>
    /// <param name="request">The request containing player and game context.</param>
    /// <returns>An availability result describing whether the action can be executed.</returns>
    public abstract Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request);
}