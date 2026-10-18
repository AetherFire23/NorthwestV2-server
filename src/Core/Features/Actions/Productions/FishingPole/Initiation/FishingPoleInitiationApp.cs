using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Initiation;

public class FishingPoleInitiationApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly FishingPoleInitiation _fishingPoleInitiation;

    public FishingPoleInitiationApp(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
        FishingPoleInitiation fishingPoleInitiation) : base(ActionNames.InitiateFishingPoleProduction)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _fishingPoleInitiation = fishingPoleInitiation;
    }

    public override async Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

        InstantActionAvailability? availability = _fishingPoleInitiation.DetermineAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

        _fishingPoleInitiation.Execute(player);

        await _unitOfWork.SaveChangesAsync();
    }
}