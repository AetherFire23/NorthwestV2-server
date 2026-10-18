using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Initiation;

public class FishingPoleInitiation
{
    public InstantActionAvailability? DetermineAvailability(Player player)
    {
        ItemInRoomRequirement scrapInRoom = new ItemInRoomRequirement(player.Room, ItemTypes.Scrap);
        PlayerInRoomRequirement playerInRoomRequirement = new PlayerInRoomRequirement(player, RoomEnum.Workshop);

        InstantActionAvailability availability = new()
        {
            ActionName = ActionNames.InitiateFishingPoleProduction,
            DisplayName = "Start",
            ActionRequirements = [scrapInRoom, playerInRoomRequirement]
        };

        return availability;
    }

    public void Execute(Player player)
    {
        // Create the unfinished fishing pole
        UnfinishedFishingPole fishingPole = new UnfinishedFishingPole();

        player.Room.Inventory.Add(fishingPole);
    }
}