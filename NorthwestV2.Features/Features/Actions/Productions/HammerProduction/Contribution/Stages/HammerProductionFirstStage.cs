using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Initiation;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Contribution.Stages;

public record HammerProductionFirstStage : StageContributionBase
{
    public const string Description = "Contribution to hammer's first stage";
    public const int REQUIRED_TIME_POINTS = 8;

    public HammerProductionFirstStage() : base(REQUIRED_TIME_POINTS, Description, Roles.Engineer)
    {
    }

    public override List<ActionRequirement> GetRequirements(Player player)
    {
        PlayerInRoomRequirement playerInRoomRequirement = new(player, HammerProductionInitiation.REQUIRED_ROOM);
        ItemInRoomRequirement itemInRoomRequirement = new(player.Room, HammerProductionInitiation.REQUIRED_ITEM);

        // TODO: seed the GetRequirements for timepoints because it shouldn't be controlled here. 
        int actionCost = this.CalculateCostForContribution(player);
        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, actionCost);

        return [playerHasTimePointsRequirement, itemInRoomRequirement, playerHasTimePointsRequirement];
    }
}