using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.General.Movement;

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


    public void ChangeRoom(Player player, List<List<ActionTarget>> roomTargetCandidate)
    {
        // TODO: May need to consider to add includes inside repositories. 
        Room room = ExtractRoomFromActionTargets(player.Game.Rooms, roomTargetCandidate);
        
        player.Room = room;
    }

    public Room ExtractRoomFromActionTargets(List<Room> allRooms, List<List<ActionTarget>> roomTargetCandidate)
    {
        ActionTarget actionTarget = roomTargetCandidate.First().First();
        
        Room room = allRooms.First(x => x.Id == actionTarget.TargetId);

        return room;
    }
}