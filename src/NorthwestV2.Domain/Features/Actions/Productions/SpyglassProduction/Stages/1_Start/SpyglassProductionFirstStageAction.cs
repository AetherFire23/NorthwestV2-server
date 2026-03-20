using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

public class SpyglassProductionFirstStageAction
{
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        PlayerInRoomRequirement isInArmory = new PlayerInRoomRequirement(player, RoomEnum.Armory);

        PlayerHasItemsRequirement isHoldingScrap = new PlayerHasItemsRequirement(player, ItemTypes.Scrap);

        InstantActionAvailability availability = new InstantActionAvailability()
        {
            ActionName = ActionNames.SpyglassProductionStart,
            DisplayName = "Start a spyglass production",
            ActionRequirements = [isHoldingScrap, isInArmory]
        };

        return availability;
    }

    // TODO: TEST IF ALL THE ADDINGS OF ALL THE ENTITIES ACTUALLY WORK ! 
    public void InitiateProduction(Player player)
    {
        Production production = new();

        Item scrapItem = player.Inventory.GetFirst(ItemTypes.Scrap);
        // Locked required Items 

        scrapItem.LockForProduction(production);

        player.Inventory.Add(new UnfinishedSpyglass());
    }

    public void Cancel()
    {
        // Destroy the unfinished item

        // Release the locked items back into the room's inventory
    }
}