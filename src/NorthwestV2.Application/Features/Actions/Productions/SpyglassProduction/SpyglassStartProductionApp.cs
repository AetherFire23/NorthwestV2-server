using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction;

public class SpyglassStartProductionApp : InstantActionBase
{
    private readonly SpyglassProductionFirstStageAction _spyglassProductionFirstStageAction;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly IProductionRepository _productionRepository;
    // TODO: production repository 

    public SpyglassStartProductionApp(SpyglassProductionFirstStageAction spyglassProductionFirstStageAction, IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository, IProductionRepository productionRepository) : base(
        ActionNames
            .SpyglassProductionStart)
    {
        _spyglassProductionFirstStageAction = spyglassProductionFirstStageAction;
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _productionRepository = productionRepository;
    }

    public async override Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        // Production production = await _productionRepository.GetProduction()
        InstantActionAvailability availability = _spyglassProductionFirstStageAction.DetermineAvailability(player);
        // var availability = _spyglassProductionAction.DetermineAvailability(player,);

        // tdd that shit
        // given scrap in room and player can start productionc x

        return availability;
    }

    public async override Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}