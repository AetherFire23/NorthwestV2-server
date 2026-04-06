using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Domain.General.Movement;

public class ChangeRoomAction
{
    // TODO: Get ActionWithTargets availabilities 

    public ActionWithTargetsAvailability CreateActionFromAvailableRooms(List<Room> adjacentRooms)
    {
        // TODO: Throw exception if the rooms are not really adjacent. 

        List<ActionRequirement> actionRequirements = CreateRequirements(adjacentRooms);

        TargetSelectionPrompt adjacentRoomSelectionPrompt = TargetSelectionPrompt.FromRooms(adjacentRooms);

        ActionWithTargetsAvailability actionWithTargetsAvailability = new ActionWithTargetsAvailability()
        {
            ActionName = ActionNames.ChangeRoom,
            DisplayName = ActionNames.ChangeRoom,
            ActionRequirements = actionRequirements,
            TargetSelectionPrompts = [adjacentRoomSelectionPrompt]
        };

        return actionWithTargetsAvailability;
    }

    private List<ActionRequirement> CreateRequirements(List<Room> rooms)
    {
        // Another room must be adjacent to this room. 
        // Insert all other movement conditions here. 

        bool isAvailableAdjacentRoom = rooms.Count > 0;
        ActionRequirement adjacentRoomRequirement = new ActionRequirement()
        {
            Description = "An adjacent room is available.",
            IsFulfilled = isAvailableAdjacentRoom
        };

        return [adjacentRoomRequirement];
    }


    public void ChangeRoom(Player player, Room room)
    {
        //TODO: verify if they really are adjacent here. 

        player.Room = room;
    }
}