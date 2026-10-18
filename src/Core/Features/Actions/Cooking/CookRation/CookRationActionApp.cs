using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.Cooking.CookRation;

public class CookRationActionApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly CookRationAction _cookRationAction;

    public CookRationActionApp(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
        CookRationAction cookRationAction) : base(ActionNames.CookRation)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _cookRationAction = cookRationAction;
    }

    public override async Task<InstantActionAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

        InstantActionAvailability? availability = _cookRationAction.DetermineAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

        _cookRationAction.Execute(player);

        await _unitOfWork.SaveChangesAsync();
    }
}
