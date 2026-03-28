using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

public class SpyglassProductionInitiationAction
{
    public const RoomEnum REQUIRED_ROOM_SPYGLASS_START = RoomEnum.Armory;
    public const ItemTypes REQUIRED_ITEM_TYPE_SPYGLASS_START = ItemTypes.Scrap;

    /// <summary>
    /// The required room is Armory. The player needs to have the spyglass in his inventory when
    /// he wants to execute the action. 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        PlayerInRoomRequirement isInArmory = new PlayerInRoomRequirement(player, REQUIRED_ROOM_SPYGLASS_START);

        PlayerHasItemsRequirement isHoldingScrap =
            new PlayerHasItemsRequirement(player, REQUIRED_ITEM_TYPE_SPYGLASS_START);

        // TODO: TimePoints requirements 
        InstantActionAvailability availability = new InstantActionAvailability
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
        Scrap scrapItemInPlayersInventory = player.Inventory.Find<Scrap>();

        UnfinishedSpyglass createdUnfinishedSpyglass = UnfinishedSpyglass.CreateFromItemsAndLock(scrapItemInPlayersInventory);
        
        // Add to room's inventory. 
        player.Room.Inventory.Add(createdUnfinishedSpyglass);
    }

    public void Cancel()
    {
        // Destroy the unfinished item

        // Release the locked items back into the room's inventory
    }
}