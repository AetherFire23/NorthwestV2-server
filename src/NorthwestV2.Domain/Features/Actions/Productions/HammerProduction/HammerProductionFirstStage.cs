using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction;

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