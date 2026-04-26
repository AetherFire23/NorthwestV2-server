using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.General.Movement;

public class ChangeRoomApp : ActionWithTargetsBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ChangeRoomAction _changeRoomAction;

    // TODO: Import changeroom domain action. 
    public ChangeRoomApp(IUnitOfWork unitOfWork, IRoomRepository roomRepository, ChangeRoomAction changeRoomAction,
        IPlayerRepository playerRepository) :
        base(ActionNames.ChangeRoom)
    {
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _changeRoomAction = changeRoomAction;
        _playerRepository = playerRepository;
    }

    public override async Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        List<Room> adjacentRooms = await _roomRepository.GetAdjacentRoomsOfPlayer(request.PlayerId);

        ActionWithTargetsAvailability changeRoomAvailability =
            _changeRoomAction.CreateActionFromAvailableRooms(adjacentRooms);
        
        var player = await _playerRepository.GetPlayer(request.PlayerId);
        



        return changeRoomAvailability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);

         _changeRoomAction.ChangeRoom(player, request.ActionTargets);
   

        await _unitOfWork.SaveChangesAsync();
    }
}