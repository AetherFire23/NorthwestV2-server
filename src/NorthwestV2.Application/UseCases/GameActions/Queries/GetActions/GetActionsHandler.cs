using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using Mediator;
using NorthwestV2.Application.UseCases.GameActions.Services;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsHandler : IRequestHandler<GetActionsRequest, GetActionsResult>
{
    private readonly ActionServices _actionServices;

    public GetActionsHandler(ActionServices actionServices)
    {
        _actionServices = actionServices;
    }


    public async ValueTask<GetActionsResult> Handle(GetActionsRequest request, CancellationToken cancellationToken)
    {
        List<InstantActionAvailability> instantActionAvailabilities = await _actionServices.GetInstantActionAvailabilityResults(request);
        
        List<ActionWithTargetsAvailability> actionWithTargetsAvailabilities = await _actionServices.GetActionWithTargetsAvailabilityResults(request);

        return new GetActionsResult
        {
            ActionWithTargets = actionWithTargetsAvailabilities,
            InstantActions = instantActionAvailabilities,
        };
    }
}