using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

/// <summary>
/// Exists because we receive the same request whether it is an instant action or an action with targets.
/// So we can collapse the request into actionbase
/// </summary>
public abstract class ActionBase
{
    public string ActionName { get; private set; }
    protected readonly NorthwestContext Context;
    
    protected ActionBase(NorthwestContext context, string actionName)
    {
        Context = context;
        ActionName = actionName;
    }

    public abstract Task Execute(ExecuteActionRequest request);
}