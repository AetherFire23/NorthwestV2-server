using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;

public class FishingPoleContribution
{
    public InstantActionAvailability DetermineAvailability(Player player)
    {
        if (!player.Room.Has<UnfinishedFishingPole>())
        {
            return null;
        }

        UnfinishedFishingPole unfinishedFishingPole = player.Room.Inventory.Find<UnfinishedFishingPole>();

        List<ActionRequirement> requirements = unfinishedFishingPole.CurrentStageContribution.GetRequirements(player);

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