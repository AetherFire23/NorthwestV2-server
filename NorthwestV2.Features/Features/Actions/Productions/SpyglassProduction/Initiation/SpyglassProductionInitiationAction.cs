using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Initiation;

public class SpyglassProductionInitiationAction
{
    public const RoomEnum REQUIRED_ROOM_SPYGLASS_START = RoomEnum.Workshop;
    public const ItemTypes REQUIRED_ITEM_TYPE_SPYGLASS_START = ItemTypes.Scrap;

    /// <summary>
    /// The required room is Workshop. The player needs to have the spyglass in his inventory when
    /// he wants to execute the action. 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        PlayerInRoomRequirement isInWorkshop = new PlayerInRoomRequirement(player, REQUIRED_ROOM_SPYGLASS_START);

        // TODO: Room has items Requirement
        PlayerHasItemsRequirement isHoldingScrap =
            new PlayerHasItemsRequirement(player, REQUIRED_ITEM_TYPE_SPYGLASS_START);

        
        InstantActionAvailability availability = new InstantActionAvailability
        {
            ActionName = ActionNames.SpyglassProductionStart,
            DisplayName = "Start a spyglass production",
            ActionRequirements = [isHoldingScrap, isInWorkshop]
        };

        return availability;
    }

    public void InitiateProduction(Player player)
    {
        Scrap scrapItemInPlayersInventory = player.Inventory.Find<Scrap>();

        UnfinishedSpyglass createdUnfinishedSpyglass =
            UnfinishedSpyglass.CreateFromItemsAndLock(scrapItemInPlayersInventory);

        // Add to room's inventory. 
        player.Room.Inventory.Add(createdUnfinishedSpyglass);
    }
}