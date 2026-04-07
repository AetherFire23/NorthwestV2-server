using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.CancelProduction;

public class CancelProductionAction
{
    public ActionWithTargetsAvailability GetAvailability(Player player)
    {
        List<ItemBase> productionItemsInRoom =
            player.Room.Inventory.Items
                .OfType<ProductionItemBase>()
                .Cast<ItemBase>()
                .ToList();

        ActionRequirement actionRequirement = new()
        {
            IsFulfilled = productionItemsInRoom.Count > 0,
            Description = "A production item exists in the current room."
        };

        TargetSelectionPrompt promptOfProductionItems = TargetSelectionPrompt.FromItems(productionItemsInRoom);

        ActionWithTargetsAvailability instant = new()
        {
            ActionName = ActionNames.CancelProduction,
            DisplayName = "Cancel Production",
            ActionRequirements = [actionRequirement],
            TargetSelectionPrompts = [promptOfProductionItems]
        };

        return instant;
    }

    public ProductionItemBase CancelProduction(Player player, Room playersRoom, ActionTargetsList actionTargets)
    {
        ProductionItemBase productionItem = ExtractProductionItemFromTargets(player, actionTargets);

        // The room of the player takes the locked items back into it
        playersRoom.Inventory.TakeOwnership(productionItem.LockedItems);

        productionItem.UnlockAllLockedItems();

        playersRoom.RemoveItem(productionItem);

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