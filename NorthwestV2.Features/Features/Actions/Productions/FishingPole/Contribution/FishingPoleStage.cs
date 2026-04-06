using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;

public record FishingPoleStage : StageContributionBase
{
    public FishingPoleStage() : base(15, "end", Roles.Captain)
    {
    }

    public override List<ActionRequirement> GetRequirements(Player player)
    {
        // Has the required 


        return [];
    }
}