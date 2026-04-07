using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Initiation;

public class FishingPoleInitiation
{
    public InstantActionAvailability? DetermineAvailability(Player player)
    {
        InstantActionAvailability availability = new()
        {
            ActionName = ActionNames.InitiateFishingPoleProduction,
            DisplayName = "Start"
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