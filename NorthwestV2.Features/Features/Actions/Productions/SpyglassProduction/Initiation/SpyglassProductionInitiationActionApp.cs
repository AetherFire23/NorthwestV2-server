using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Instant;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Initiation;

public class SpyglassProductionInitiationActionApp : InstantActionBase
{
    private readonly SpyglassProductionInitiationAction _spyglassProductionInitiationAction;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly IProductionRepository _productionRepository;
    // TODO: production repository 

    public SpyglassProductionInitiationActionApp(SpyglassProductionInitiationAction spyglassProductionInitiationAction,
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