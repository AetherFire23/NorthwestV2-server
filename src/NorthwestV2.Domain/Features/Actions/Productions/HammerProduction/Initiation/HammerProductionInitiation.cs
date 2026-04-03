using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;

public class HammerProductionInitiation
{
    public const RoomEnum REQUIRED_ROOM = RoomEnum.Workshop;
    public const ItemTypes REQUIRED_ITEM = ItemTypes.Scrap;

    /// <summary>
    /// 1. Workshop room
    /// 2. Low Engineer TP
    /// 3. Scrap 
    /// </summary>
    /// <returns></returns>
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        PlayerInRoomRequirement playerInRoomRequirement = new(player, REQUIRED_ROOM);
        ItemInRoomRequirement itemInRoomRequirement = new(player.Room, REQUIRED_ITEM);

        //TODO: Demander si initier une production ca coute dequoi ou bedon si c'est pas mal gratuit. 
        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, 1);

        InstantActionAvailability instant = new()
        {
            ActionName = ActionNames.HammerProductionInitiation,
            DisplayName = "Initiate hammer production",
            ActionRequirements = [playerInRoomRequirement, itemInRoomRequirement, playerHasTimePointsRequirement]
        };

        return instant;
    }

    public void Execute(Player player)
    {
        // TODO: make more abstract logic for this
        Scrap scrap = player.Inventory.Find<Scrap>();

        UnfinishedHammer unfinishedHammer = UnfinishedHammer.CreateFromItemsAndLock(scrap);

        player.Room.Inventory.Add(unfinishedHammer);
    }
}