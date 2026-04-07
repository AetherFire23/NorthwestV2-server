using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;

public class FishingPoleContribution
{
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        // TODO: Cleanup
        var unf = player.Room.Inventory.Items
            .FirstOrDefault(x => x is UnfinishedFishingPole) as UnfinishedFishingPole;

        if (unf is null)
        {
            return null;
        }

        List<ActionRequirement> requirements = unf.CurrentStageContribution.GetRequirements(player);

        InstantActionAvailability instantActionAvailability = new InstantActionAvailability()
        {
            ActionName = ActionNames.ContributeFishingPoleProduction,
            DisplayName = " Contribute points to the fishing pole production.",
            ActionRequirements = requirements,
        };

        return instantActionAvailability;
    }

    public void Execute()
    {
    }
}