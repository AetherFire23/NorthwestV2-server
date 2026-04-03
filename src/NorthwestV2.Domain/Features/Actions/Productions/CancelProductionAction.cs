using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions;

public class CancelProductionAction
{
    public ActionWithTargetsAvailability GetAvailability(Player player)
    {
        RoomHasItemRequirement roomHasUnfinishedSpyglass = new(player.Room, ItemTypes.UnfinishedSpyglass);

        List<ItemBase> productionItemsInRoom =
            player.Room.Inventory.Items
                .OfType<ProductionItemBase>()
                .Cast<ItemBase>()
                .ToList();

        TargetSelectionPrompt promptOfProductionItems = TargetSelectionPrompt.FromItems(productionItemsInRoom);

        ActionWithTargetsAvailability instant = new()
        {
            ActionName = ActionNames.CancelProduction,
            DisplayName = "Cancel Production",
            ActionRequirements = [roomHasUnfinishedSpyglass],
            TargetSelectionPrompts = [promptOfProductionItems]
        };

        return instant;
    }

    public ProductionItemBase CancelProduction(Player player,
        ActionTargetsList actionTargets)
    {
        ProductionItemBase productionItem = ExtractProductionItemFromTargets(player, actionTargets);

        player.Room.Inventory.TakeOwnership(productionItem.LockedItems);

        player.Room.Inventory.Items.Remove(productionItem);

        return productionItem;
    }

    private static ProductionItemBase ExtractProductionItemFromTargets(Player player, ActionTargetsList actionTargets)
    {
        // Getting the only possible production targettable when cancelling a production. 
        ActionTarget productionTarget = actionTargets.Single();

        // Getting the production item from the targetId
        ProductionItemBase productionItem =
            (ProductionItemBase)player.Room.Inventory.FindById(productionTarget.TargetId.Value);
        return productionItem;
    }
}