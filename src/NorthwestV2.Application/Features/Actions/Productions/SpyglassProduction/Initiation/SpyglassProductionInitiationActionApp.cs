using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

public class SpyglassProductionInitiationActionApp : InstantActionBase
{
    private readonly AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start.SpyglassProductionInitiationAction _spyglassProductionInitiationAction;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly IProductionRepository _productionRepository;
    // TODO: production repository 

    public SpyglassProductionInitiationActionApp(AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start.SpyglassProductionInitiationAction spyglassProductionInitiationAction,
        IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository, IProductionRepository productionRepository) : base(
        ActionNames
            .SpyglassProductionStart)
    {
        _spyglassProductionInitiationAction = spyglassProductionInitiationAction;
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _productionRepository = productionRepository;
    }

    public async override Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        InstantActionAvailability availability = _spyglassProductionInitiationAction.DetermineAvailability(player);
  
        return availability;
    }

    public async override Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);
        
        _spyglassProductionInitiationAction.InitiateProduction(player);
        
        await _unitOfWork.SaveChangesAsync();
    }
}