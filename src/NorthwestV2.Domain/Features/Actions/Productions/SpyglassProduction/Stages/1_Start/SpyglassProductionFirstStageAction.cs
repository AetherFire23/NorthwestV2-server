using System.Diagnostics.CodeAnalysis;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

public class SpyglassProductionFirstStageAction
{
    public const RoomEnum REQUIRED_ROOM_SPYGLASS_START = RoomEnum.Armory;
    public const ItemTypes REQUIRED_ITEM_TYPE_SPYGLASS_START = ItemTypes.Scrap;

    public InstantActionAvailability DetermineAvailability(Player player)
    {
        PlayerInRoomRequirement isInArmory = new PlayerInRoomRequirement(player, REQUIRED_ROOM_SPYGLASS_START);

        PlayerHasItemsRequirement isHoldingScrap =
            new PlayerHasItemsRequirement(player, REQUIRED_ITEM_TYPE_SPYGLASS_START);

        // TODO: TimePoints requirements 
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
        Scrap scrapItem = player.Inventory.Find<Scrap>();

        UnfinishedSpyglass unfinishedSpyglass = UnfinishedSpyglass.CreateFromItemsAndLock(scrapItem);
        
        player.Room.Inventory.Add(unfinishedSpyglass);
    }

    public void Cancel()
    {
        // Destroy the unfinished item

        // Release the locked items back into the room's inventory
    }
}