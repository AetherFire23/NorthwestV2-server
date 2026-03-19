using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
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

    public InstantActionAvailability DetermineAvailability()
    {
        var requirements = GetActionRequirements();

        InstantActionAvailability instantActionAvailability = new InstantActionAvailability()
        {
            ActionName = ActionNames.SpyglassProduction,
            ActionRequirements = requirements,
        };

        return instantActionAvailability;
    }

    private List<ActionRequirement> GetActionRequirements()
    {
        return [];
    }

    // Actions 

    public void InitiateProduction(Production production, Item scrapItem, Room room)
    {
        if (scrapItem.ItemType != ItemTypes.Scrap)
        {
            throw new Exception("Item type not of required type. ");
        }

        // Locked required Items 

        scrapItem.IsLocked = true;
        scrapItem.Production = production;

        room.Inventory.Items.Add(new UnfinishedSpyglass());

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