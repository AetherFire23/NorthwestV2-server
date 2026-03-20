using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

public class SpyglassFirstStage : Stage
{
    public SpyglassFirstStage() : base(RoomEnum.Workshop, [ItemTypes.Scrap])
    {
    }

    public  InstantActionAvailability CalculateAvailability(Player player)
    {
        List<ActionRequirement> spyglassInitiationRequirement =
            GetPlayerInRoomAndPlayerHoldsItemRequirements(player);

        InstantActionAvailability instant = new InstantActionAvailability()
        {
            ActionName = ActionNames.SpyglassProductionStart,
            DisplayName = "Initiate spyglass production",
            ActionRequirements = spyglassInitiationRequirement,
        };

        return instant;
    }

    private List<ActionRequirement> GetPlayerInRoomAndPlayerHoldsItemRequirements(Player player)
    {
        // Player must hold all required items
        // Player must be in room 
        PlayerInRoomRequirement playerInRoomRequirement = new PlayerInRoomRequirement(player, RoomEnum.Armory);

        ActionRequirement playerHoldsItemRequirement = new ActionRequirement()
        {
            Description = "Player holds a Scrap Item.",
            IsFulfilled = player.Inventory.Items.Any(x => x.ItemType == ItemTypes.Scrap),
        };
        return [playerInRoomRequirement, playerHoldsItemRequirement];
    }
}