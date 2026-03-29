using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages.Stages;

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
    /// <summary>
    /// The maximum number of contributions required to complete the first stage.
    /// Player must contribute a total of 8 time points to advance.
    /// </summary>
    public const int SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT = 8;
    public const string DESCRIPTION = "First stage - assembly of casings and lenses";
    public const RoomEnum REQUIRED_ROOM = RoomEnum.Workshop;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpyglassFirstStageContributionData"/> class
    /// with the contribution limit and descriptive stage name.
    /// </summary>
    public SpyglassFirstStageContributionData() : base(SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT,
        DESCRIPTION)
    {
    }

    protected override StageContributionBase? GetNextStage()
    {
        return new SpyglassSecondStageContributionData();
    }


    //
    public override List<ActionRequirement> GetRequirements(Player player)
    {
        RoomHasItemRequirement roomOfPlayerHasRequirementItem =
            new RoomHasItemRequirement(player.Room, ItemTypes.UnfinishedSpyglass);

        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, 1);

        PlayerInRoomRequirement playerInWorkshopRequirement =
            new PlayerInRoomRequirement(player, REQUIRED_ROOM);

        return [playerInWorkshopRequirement, roomOfPlayerHasRequirementItem, playerHasTimePointsRequirement];
    }
}