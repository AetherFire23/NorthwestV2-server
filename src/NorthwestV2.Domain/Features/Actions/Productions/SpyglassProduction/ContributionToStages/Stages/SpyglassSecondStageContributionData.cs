using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._3_Third;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages.Stages;

public record SpyglassSecondStageContributionData : StageContributionBase
{
    public const int SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT = 12;
    public const string DESCRIPTION = "Second stage - calibration, astronomical alignment, and observational tuning";
    public const RoomEnum REQUIRED_ROOM = RoomEnum.ForeCastle;

    public SpyglassSecondStageContributionData() : base(SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT,
        DESCRIPTION, Roles.Scholar)
    {
    }

    protected override StageContributionBase? GetNextStage()
    {
        return new SpyglassProductionThirdStageContributionData();
    }

    public override List<ActionRequirement> GetRequirements(Player player)
    {
        ItemInRoomRequirement isItemInRoomHavingUnfinishedSpyglass =
            new(player.Room, ItemTypes.UnfinishedSpyglass);

        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, CalculateCostForContribution(player));

        PlayerInRoomRequirement playerInForecastleRequirement = new(player, REQUIRED_ROOM);

        return [playerInForecastleRequirement, isItemInRoomHavingUnfinishedSpyglass, playerHasTimePointsRequirement];
    }
}