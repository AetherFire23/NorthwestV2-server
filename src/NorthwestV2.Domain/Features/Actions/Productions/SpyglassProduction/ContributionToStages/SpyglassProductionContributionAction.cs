using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages;

public class SpyglassProductionContributionAction
{
    public InstantActionAvailability? DetermineAvailability(Player player)
    {
        bool hasUnfinishedSPyglass =
            player.Room.Inventory.Items.Any(x => x.ItemType == ItemTypes.UnfinishedSpyglass);
        if (!hasUnfinishedSPyglass)
        {
            return null;
        }

        /*
         *  There is an issue where tehre could be 2 different unfinishedSpyglasess.
         * It's highly unlikely that this would be the case, however, in that case I am just picking the spyglass with
         * the highest Contributions.
         * Since there is just one spyglass production' there is an increased responsibility to build the
         * appropraite availability. ie :
         */

        UnfinishedSpyglass unfinishedSpyglass = player.Room.Inventory.Find<UnfinishedSpyglass>();

        StageContributionBase contributions = unfinishedSpyglass.CurrentStageContribution;

        List<ActionRequirement> currentRequirements = contributions.GetRequirements(player);

        RoomHasItemRequirement isRoomHavinunfinishedspyglass =
            new RoomHasItemRequirement(player.Room, ItemTypes.UnfinishedSpyglass);

        InstantActionAvailability availability = new InstantActionAvailability
        {
            ActionName = ActionNames.SpyglassContribution,
            DisplayName = $"{contributions.StageName}",
            ActionRequirements = currentRequirements
        };

        return availability;
    }

    public void Contribute(Player player)
    {
        UnfinishedSpyglass unfinishedSpyglass = player.Room.Inventory.Find<UnfinishedSpyglass>();
        unfinishedSpyglass.Contribute(player);
    }
}