using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;

namespace NorthwestV2.Application.Features.Actions.Core.Bases;

/// <summary>
/// Base class for all executable game actions. Provides shared infrastructure such as
/// access to the database context and a stable action name used for identification.
/// /// </summary>
public abstract class ActionBase
{
    /// <summary>
    /// The unique name of the action. Used for identification, and UI display.
    /// </summary>
    public string ActionName { get; private set; }




    /// <summary>
    /// Initializes a new action with the given database context and action name.
    /// </summary>
    /// <param name="context">The database context used by the action.</param>
    /// <param name="actionName">The unique name of the action.</param>
    protected ActionBase(string actionName)
    {
        ActionName = actionName;
    }

    /// <summary>
    /// Executes the action using the provided request data.
    /// Implementations modify game state and persist changes as needed.
    /// </summary>
    /// <param name="request">The execution request containing player and action data.</param>
    public abstract Task Execute(ExecuteActionRequest request);
}