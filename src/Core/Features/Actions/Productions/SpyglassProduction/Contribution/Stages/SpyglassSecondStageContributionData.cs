using NorthwestV2.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Actions.Productions.Core;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;

/// <summary>
/// Calibration stage. It asks for two specialized quotas: Scholar TP ("very high") and Sentry TP ("high").
/// The stage ends only when both are full.
/// </summary>
public record SpyglassSecondStageContributionData : StageContributionBase
{
    /// <summary>Scholar quota. The exact number is assumed, the docs only say "very high".</summary>
    public const int SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT = 12;

    /// <summary>Sentry quota. The exact number is assumed, the docs only say "high".</summary>
    public const int SPYGLASS_SECOND_STAGE_SENTRY_CONTRIBUTION_LIMIT = 8;

    public const string DESCRIPTION = "Second stage - calibration, astronomical alignment, and observational tuning";
    public const RoomEnum REQUIRED_ROOM = RoomEnum.ForeCastle;

    public SpyglassSecondStageContributionData() : base(SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT,
        DESCRIPTION, Roles.Scholar)
    {
    }

    /// <summary>
    /// The Sentry quota. <see cref="StageContributionBase.RequiredContributions"/> holds the Scholar quota.
    /// </summary>
    public int SentryContributions { get; set; }

    public override bool IsStageEnded =>
        RequiredContributions >= End && SentryContributions >= SPYGLASS_SECOND_STAGE_SENTRY_CONTRIBUTION_LIMIT;

    public override IReadOnlyList<Roles> ContributionRoles => [Roles.Scholar, Roles.Sentry];

    public override bool IsQuotaOpen(Roles quotaRole)
    {
        return quotaRole switch
        {
            Roles.Scholar => RequiredContributions < End,
            Roles.Sentry => SentryContributions < SPYGLASS_SECOND_STAGE_SENTRY_CONTRIBUTION_LIMIT,
            _ => false
        };
    }

    public override StageContributionBase WithContribution(Roles quotaRole)
    {
        return quotaRole == Roles.Sentry
            ? this with { SentryContributions = SentryContributions + 1 }
            : base.WithContribution(quotaRole);
    }

    protected override StageContributionBase? GetNextStage()
    {
        return new SpyglassProductionThirdStageContributionData();
    }

    public override List<ActionRequirement> GetRequirements(Player player)
    {
        return GetRequirements(player, Roles.Scholar);
    }

    public override List<ActionRequirement> GetRequirements(Player player, Roles quotaRole)
    {
        ItemInRoomRequirement isItemInRoomHavingUnfinishedSpyglass =
            new(player.Room, ItemTypes.UnfinishedSpyglass);

        PlayerHasTimePointsRequirement playerHasTimePointsRequirement =
            PlayerHasTimePointsRequirement.Create(player, CalculateCostForContribution(player, quotaRole));

        ActionRequirement quotaIsOpen = new($"The {quotaRole} quota of this stage is not full yet.",
            IsQuotaOpen(quotaRole));

        PlayerInRoomRequirement playerInForecastleRequirement = new(player, REQUIRED_ROOM);

        return
        [
            playerInForecastleRequirement, isItemInRoomHavingUnfinishedSpyglass, quotaIsOpen,
            playerHasTimePointsRequirement
        ];
    }
}
