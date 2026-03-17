using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Movement;

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