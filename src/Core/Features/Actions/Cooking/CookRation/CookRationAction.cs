using NorthwestV2.Features.Actions.Cooking.Items;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.Cooking.CookRation;

/// <summary>
/// Turns one can into one ration, in any room, for 0 TP. Eating effects (sickness, toxicity) are not part of this action.
/// </summary>
public class CookRationAction
{
    public InstantActionAvailability? DetermineAvailability(Player player)
    {
        PlayerHasItemsRequirement hasCan = new PlayerHasItemsRequirement(player, ItemTypes.Can);

        InstantActionAvailability availability = new()
        {
            ActionName = ActionNames.CookRation,
            DisplayName = "Cook a ration",
            ActionRequirements = [hasCan]
        };

        return availability;
    }

    public void Execute(Player player)
    {
        ItemBase can = player.Inventory.Find(ItemTypes.Can);

        player.Inventory.Items.Remove(can);
        player.Inventory.Add(new Ration());
    }
}
