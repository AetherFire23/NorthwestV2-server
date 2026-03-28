using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages;

public class SpyglassProductionContributionAction
{
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        RoomHasItemRequirement isRoomHavinunfinishedspyglass =
            new RoomHasItemRequirement(player.Room, ItemTypes.UnfinishedSpyglass);

        // TODO: get the current contribution's stage 
        InstantActionAvailability availability = new InstantActionAvailability
        {
            ActionName = ActionNames.SpyglassContribution,
            DisplayName = $"Contribute to spyglass production: ",
            ActionRequirements = [isRoomHavinunfinishedspyglass]
        };

        return availability;
    }

    public void Contribute(Player player)
    {
        UnfinishedSpyglass unfinishedSpyglass = player.Room.Inventory.Find<UnfinishedSpyglass>();
        unfinishedSpyglass.Contribute(player);
    }
}