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

        List<ActionTarget> productionItemsInRoom =
            player.Room.Inventory.Items
                .OfType<ProductionItemBase>()
                .Select(x => x.ToActionTarget())
                .ToList();


        ActionWithTargetsAvailability instant = new()
        {
            ActionName = ActionNames.CancelProduction,
            DisplayName = "Cancel Production",
            ActionRequirements = [roomHasUnfinishedSpyglass],
            TargetSelectionPrompts = [[]]
        };

        return instant;
    }

    public void CancelProduction(Player player, List<List<ActionTarget>> actionTargets)
    {
        ActionTarget production = actionTargets.First().Single();

        player.Room.Inventory.Items.First(x => x.Id == production.TargetId);
    }
}