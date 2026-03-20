using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;

public class SpyglassProductionSecondStageAction
{
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        // TODO: Verify if the correct room; 
        PlayerInRoomRequirement playerInRoomRequiremnt = new(player, RoomEnum.Armory);
        PlayerHasItemsRequirement playerHasItemsRequirement = new(player, ItemTypes.UnfinishedSpyglass);

        InstantActionAvailability availability = new()
        {
            ActionName = ActionNames.SpyglassProductionSecond,
            DisplayName = "Spyglass - Second stage",
            ActionRequirements = [playerHasItemsRequirement, playerInRoomRequiremnt]
        };

        return availability;
    }
}