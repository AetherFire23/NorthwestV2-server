using Mediator;
using NorthwestV2.Features.Features.Actions.Core.Application;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;

namespace NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

public class GetActionsHandler : IRequestHandler<GetActionsRequest, GetActionsResult>
{
    private readonly ActionServices _actionServices;

    public GetActionsHandler(ActionServices actionServices)
    {
        _actionServices = actionServices;
    }


    public async ValueTask<GetActionsResult> Handle(GetActionsRequest request, CancellationToken cancellationToken)
    {
        List<InstantActionAvailability> instantActionAvailabilities =
            await _actionServices.GetInstantActionAvailabilityResults(request);

        List<ActionWithTargetsAvailability> actionWithTargetsAvailabilities =
            await _actionServices.GetActionWithTargetsAvailabilityResults(request);

        IEnumerable<ActionDto> instantActionsAsDto = instantActionAvailabilities
            .Select(ActionDto.FromInstant);

        IEnumerable<ActionDto> actionWithTargetsDto = actionWithTargetsAvailabilities
            .Select(ActionDto.FromTarget);


        GetActionsResult getActionsResult = new GetActionsResult()
        {
            Actions = instantActionsAsDto.Union(actionWithTargetsDto).ToList(),
        };

        return getActionsResult;
    }
}