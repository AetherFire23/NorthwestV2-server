using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;

public class SpyglassSecondStageApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly SpyglassProductionSecondStageAction _spyglassProductionSecondStageAction;

    public SpyglassSecondStageApp(string actionName, IUnitOfWork unitOfWork,
        SpyglassProductionSecondStageAction spyglassProductionSecondStageAction, IPlayerRepository playerRepository) :
        base(actionName)
    {
        _unitOfWork = unitOfWork;
        _spyglassProductionSecondStageAction = spyglassProductionSecondStageAction;
        _playerRepository = playerRepository;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        InstantActionAvailability secondStageAvailability =
            _spyglassProductionSecondStageAction.DetermineAvailability(player);

        return secondStageAvailability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
    }
}