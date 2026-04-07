using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.ContributionToStages.Stages;

public record SpyglassProductionThirdStageContributionData : StageContributionBase
{
    public const int SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT = 15;
    public const string DESCRIPTION = "Third stage - final fitting and reinforcement";
    public const RoomEnum REQUIRED_ROOM = RoomEnum.Workshop;

    public SpyglassProductionThirdStageContributionData() : base(SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT,
        DESCRIPTION, Roles.Engineer)
    {
    }

    public override List<ActionRequirement> GetRequirements(Player player)
    {
        ItemInRoomRequirement isItemInRoomHavingUnfinishedSpyglass =
            new ItemInRoomRequirement(player.Room, ItemTypes.UnfinishedSpyglass);

        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, CalculateCostForContribution(player));

        PlayerInRoomRequirement playerInWorkshopRequirement =
            new PlayerInRoomRequirement(player, REQUIRED_ROOM);

        return [playerInWorkshopRequirement, isItemInRoomHavingUnfinishedSpyglass, playerHasTimePointsRequirement];
    }
}