using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction;

// Actions are determined by their base application classes ( reflection ) 
// TODO: Problem How to split into sub-actions 
public class SpyglassProductionAction
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
        List<Item> unfinishedSpyglassInRoomOrPlayer =
            player.Inventory.Items.Union(player.Room.Inventory.Items).ToList();

        bool hasUnfinished = unfinishedSpyglassInRoomOrPlayer.Any(x => x.ItemType == ItemTypes.UnfinishedSpyglass);

        bool hasScrapItem = unfinishedSpyglassInRoomOrPlayer.Any(x => x.ItemType == ItemTypes.Scrap);
        if (hasUnfinished)
        {
            // Check if we can continue the stages
        }
        else if (hasScrapItem)
        {
            // Check if we can start the production.  
        }


        int i = 0;
        // Check if there is an item in the room with the player OR in the player's inventory.
        // Check if the it's an UnfinishedSpyglass
        // If no unfinished spyglass -> check if scrap present -> if not present return nothing 
        // If scrap present && noSpyglass-> make this action available 

        // If there is an unfinished spyglass -> it's stages 2-3. (stage 1 is actually just a start condition of there is no unfinished spyglass.)
        // Stages should be stored as ValueObjects.


        // If there is a spyglass. 


        // Stage currentStage = this.Stages.ElementAt(production._currentStageIndex);
        //
        // InstantActionAvailability availability = currentStage.CalculateAvailability(player, production);

        return null;
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