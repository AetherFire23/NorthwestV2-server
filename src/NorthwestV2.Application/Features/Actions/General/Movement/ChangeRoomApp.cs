using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.General.Movement;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.General.Movement;

public class ChangeRoomApp : ActionWithTargetsBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomRepository _roomRepository;

    private readonly ChangeRoomAction _changeRoomAction;

    // TODO: Import changeroom domain action. 
    public ChangeRoomApp(IUnitOfWork unitOfWork, IRoomRepository roomRepository, ChangeRoomAction changeRoomAction) :
        base(ActionNames.ChangeRoom)
    {
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _changeRoomAction = changeRoomAction;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        // TODO: Get the current player's adjacent rooms.

        List<Room> adjacentRooms = await _roomRepository.GetAdjacentRoomsOfPlayer(request.PlayerId);

        ActionWithTargetsAvailability changeRoomAvailability =
            _changeRoomAction.CreateActionFromAvailableRooms(adjacentRooms);

        // TODO: Eventually check if the requested room exists. 

        return changeRoomAvailability;
    }

    public override Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}