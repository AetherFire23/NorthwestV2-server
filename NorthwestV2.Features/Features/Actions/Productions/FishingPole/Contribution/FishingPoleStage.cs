using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;

public record FishingPoleStage : StageContributionBase
{
    public const Roles SPECIALIZED_ROLE = Roles.Sentry;
    public const int CONTRIBUTION_LIMIT = 15;
    public const string STAGE_NAME = "Contribution to fishing pole";
    public const RoomEnum REQUIRED_ROOM = RoomEnum.Workshop;
    
    

    public FishingPoleStage() : base(CONTRIBUTION_LIMIT, STAGE_NAME, SPECIALIZED_ROLE)
    {
    }

    public override List<ActionRequirement> GetRequirements(Player player)
    {

        return [];
    }
}