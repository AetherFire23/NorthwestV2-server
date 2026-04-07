using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements
{
    public class PlayerInRoomRequirement : ActionRequirement
    {
        public PlayerInRoomRequirement(Player player, RoomEnum requiredRoom)
        {
            this.IsFulfilled = player.Room.RoomEnum == requiredRoom;
            this.Description = $"Player must be in given room {requiredRoom}";
        }
    }
}