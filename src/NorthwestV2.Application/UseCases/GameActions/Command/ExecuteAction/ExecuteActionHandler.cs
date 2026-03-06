using Mediator;
using NorthwestV2.Application.UseCases.GameActions.Services;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

namespace NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;

public class ExecuteActionHandler : IRequestHandler<ExecuteActionRequest>
{
    private readonly ActionServices _actionServices;

    public ExecuteActionHandler(ActionServices actionServices)
    {
        _actionServices = actionServices;
    }

    public async ValueTask<Unit> Handle(ExecuteActionRequest request, CancellationToken cancellationToken)
    {
        ActionBase action = await _actionServices.GetActionFromName(request.ActionName);

        if (action is ActionWithTargetsBase actionWithTargets)
        {
        }

        if (action is InstantActionBase instantAction)
        {
            
        }


        // Validate execution 
        await action.Execute(request);

        return Unit.Value;
    }
}