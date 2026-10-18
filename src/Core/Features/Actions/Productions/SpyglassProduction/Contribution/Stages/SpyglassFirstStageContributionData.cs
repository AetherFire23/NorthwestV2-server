using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;

/// <summary>
/// Represents the first production stage for crafting a spyglass.
/// This stage focuses on assembling the casings and lenses components.
/// </summary>
/// <remarks>
/// Inherits from <see cref="StageContributionBase"/> and defines the contribution limit and 
/// stage name for the initial phase of spyglass production.
/// </remarks>
public record SpyglassFirstStageContributionData : StageContributionBase
{
    public const int SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT = 8;
    public const string DESCRIPTION = "First stage - assembly of casings and lenses";
    public const RoomEnum REQUIRED_ROOM = RoomEnum.Workshop;


    /// <summary>
    /// Initializes a new instance of the <see cref="SpyglassFirstStageContributionData"/> class
    /// with the contribution limit and descriptive stage name.
    /// </summary>
    public SpyglassFirstStageContributionData() : base(SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT,
        DESCRIPTION, Roles.Engineer)
    {
    }

    protected override StageContributionBase? GetNextStage()
    {
        return new SpyglassSecondStageContributionData();
    }


    public override List<ActionRequirement> GetRequirements(Player player)
    {
        ItemInRoomRequirement itemInRoomOfPlayerHasRequirementItem =
            new ItemInRoomRequirement(player.Room, ItemTypes.UnfinishedSpyglass);

        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, CalculateCostForContribution(player));

        PlayerInRoomRequirement playerInWorkshopRequirement =
            new PlayerInRoomRequirement(player, REQUIRED_ROOM);

        return [playerInWorkshopRequirement, itemInRoomOfPlayerHasRequirementItem, playerHasTimePointsRequirement];
    }
}