using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

// Actions are determined by their base application classes ( reflection ) 
// TODO: Problem How to split into sub-actions 
public class SpyglassProductionFirstStageAction
{
    public List<Stage> Stages = [new SpyglassFirstStage()];

    /// <summary>
    /// Will send differfent information depending on the current availability.
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
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

    // TODO: Check how many other productions have the same shape. 
    public void InitiateProduction(Production production, Item scrapItem, Room room)
    {
        if (scrapItem.ItemType != ItemTypes.Scrap)
        {
            throw new Exception("Item type not of required type. ");
        }

        // Locked required Items 

        scrapItem.IsLocked = true;
        scrapItem.Production = production;

        // room.Inventory.Items.Add(new UnfinishedSpyglass());

        // Lock old item 
    }

    public void Cancel()
    {
        // Destroy the unfinished item

        // Release the locked items back into the room's inventory
    }

    public Item Completion()
    {
        Spyglass item = new Spyglass();

        return item;
    }
}