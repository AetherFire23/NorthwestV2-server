using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Initiation;

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

