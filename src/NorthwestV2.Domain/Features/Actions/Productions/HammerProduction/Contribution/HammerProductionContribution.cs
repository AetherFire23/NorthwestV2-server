using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Contribution;

public class HammerProductionContribution
{
    public InstantActionAvailability? DetermineAvailability(Player player)
    {
        if (!player.Room.Has<UnfinishedHammer>())
        {
            return null;
        }

        UnfinishedHammer unfinishedItem = player.Room.Inventory.Find<UnfinishedHammer>();

        List<ActionRequirement> requirements = unfinishedItem.CurrentStageContribution.GetRequirements(player);

        InstantActionAvailability availability = new()
        {
            ActionName = ActionNames.HammerProductionContribution,
            DisplayName = "Contribute to the the hammer item contribution. ",
            ActionRequirements = requirements,
        };

        return availability;
    }

    /*
     * C'est quoi une contribution ? :
     * 1. Pogner l'item dans la room.
     * 2. contribuer au stage.
     *
     */
    public void Execute(Player player)
    {
        UnfinishedHammer unfinishedHammer = player.Room.Inventory.Find<UnfinishedHammer>();

        unfinishedHammer.Contribute(player);
    }
}