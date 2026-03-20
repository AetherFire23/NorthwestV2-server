using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

public class PlayerInRoomRequirement : ActionRequirement
{
    private readonly Player _player;
    private readonly RoomEnum _requiredRoom;

    public PlayerInRoomRequirement(Player player, RoomEnum requiredRoom)
    {
        _player = player;
        _requiredRoom = requiredRoom;
    }

    public override bool IsFulfilled => this._player.Room.RoomEnum == _requiredRoom;
    public override string Description => $"Player must be in given room {_requiredRoom}";
}