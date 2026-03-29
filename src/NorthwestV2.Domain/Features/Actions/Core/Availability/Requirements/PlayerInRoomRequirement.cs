using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

public class PlayerInRoomRequirement : ActionRequirement
{
    public PlayerInRoomRequirement(Player player, RoomEnum requiredRoom)
    {
        this.IsFulfilled = player.Room.RoomEnum == requiredRoom;
        this.Description = $"Player must be in given room {requiredRoom}";
    }
}