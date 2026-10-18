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
        // Get the instant actions. 
        IEnumerable<ActionDto> instantActionsAsDto = await GetInstantActionDtos(request);

        // Get the actions with targets. 
        IEnumerable<ActionDto> actionWithTargetsDto = await GetActionsWithTargetsDtos(request);

        GetActionsResult getActionsResult = new GetActionsResult()
        {
            Actions = instantActionsAsDto.Union(actionWithTargetsDto).ToList(),
        };

        return getActionsResult;
    }

    private async ValueTask<IEnumerable<ActionDto>> GetActionsWithTargetsDtos(GetActionsRequest request)
    {
        List<ActionWithTargetsAvailability> actionWithTargetsAvailabilities =
            await _actionServices.GetActionWithTargetsAvailabilityResults(request);
        IEnumerable<ActionDto> actionWithTargetsDto = actionWithTargetsAvailabilities
            .Select(ActionDto.FromTarget);
        return actionWithTargetsDto;
    }

    private async ValueTask<IEnumerable<ActionDto>> GetInstantActionDtos(GetActionsRequest request)
    {
        List<InstantActionAvailability> instantActionAvailabilities =
            await _actionServices.GetInstantActionAvailabilityResults(request);
        IEnumerable<ActionDto> instantActionsAsDto = instantActionAvailabilities
            .Select(ActionDto.FromInstant);
        return instantActionsAsDto;
    }
}