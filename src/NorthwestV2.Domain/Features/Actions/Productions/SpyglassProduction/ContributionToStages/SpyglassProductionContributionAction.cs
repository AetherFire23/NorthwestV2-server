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
        /*
         *  There is an issue where tehre could be 2 different unfinishedSpyglasess.
         * It's highly unlikely that this would be the case, however, in that case I am just picking the spyglass with
         * the highest Contributions.
         * Since there is just one spyglass production' there is an increased responsibility to build the
         * appropraite availability. ie :
         */

        // Get the current stage (If it exists)
        
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

        if (unfinishedSpyglass.CurrentStage.IsProductionComplete)
        {
            // TODO: 
            // Create the finished version in the player's inventory ! 
        }
    }
}