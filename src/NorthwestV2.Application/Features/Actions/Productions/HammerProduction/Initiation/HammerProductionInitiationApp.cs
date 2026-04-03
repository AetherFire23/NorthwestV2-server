using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.HammerProduction.Initiation;

public class HammerProductionInitiationApp : InstantActionBase
{
    private readonly HammerProductionInitiation _hammerProductionInitiation;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HammerProductionInitiationApp(HammerProductionInitiation hammerProductionInitiation,
        IPlayerRepository playerRepository, IUnitOfWork unitOfWork) : base(ActionNames
        .HammerProductionInitiation)
    {
        _hammerProductionInitiation = hammerProductionInitiation;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

        InstantActionAvailability instantActionAvailability = _hammerProductionInitiation.DetermineAvailability(player);

        return instantActionAvailability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);
        
        // Create the UnfinishedHammer in the room's inventory. 
        
        _hammerProductionInitiation.Execute(player);

        await _unitOfWork.SaveChangesAsync();
    }
}

