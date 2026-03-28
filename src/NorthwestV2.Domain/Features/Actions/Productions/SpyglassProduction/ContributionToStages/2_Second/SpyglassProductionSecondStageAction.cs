using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._2_Second;

// TODO: Should be under Contribution action
public class SpyglassProductionSecondStageAction
{
    public const int TIME_POINTS_REQUIREMENTS_SPYGLASS_SECONDSTAGE_ACTION = 2;

    public InstantActionAvailability DetermineAvailability(Player player)
    {
        // TODO: Verify if the correct room; 
        PlayerInRoomRequirement playerInRoomRequiremnt = new(player, RoomEnum.ForeCastle);
        PlayerHasItemsRequirement playerHasItemsRequirement = new(player, ItemTypes.UnfinishedSpyglass);
        PlayerHasTimePointsRequirement hasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, TIME_POINTS_REQUIREMENTS_SPYGLASS_SECONDSTAGE_ACTION);

        InstantActionAvailability availability = new()
        {
            ActionName = ActionNames.SpyglassProductionSecond,
            DisplayName = "Spyglass - Second stage",
            ActionRequirements =
            [
                playerHasItemsRequirement,
                playerInRoomRequiremnt,
                playerInRoomRequiremnt,
                hasTimePointsRequirement
            ]
        };

        return availability;
    }

    // TODO: Calculate time points based on specific roles.

    public void ContributePointsToProduction(Player player)
    {
        /*
         * How contributing works :
         *
         */
        ItemBase unfinishedSpyglassProduction = player.Inventory.Find(ItemTypes.UnfinishedSpyglass);

        // add points;

        // Check how many points it should ahve at the end of this stage (6,7)
    }
}