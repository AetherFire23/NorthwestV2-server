using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._3_Third;

public record SpyglassProductionThirdStageContributionData : StageContributionBase
{
    public const int SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT = 15;
    public const string THIRD_STAGE_NAME = "Third stage";

    public SpyglassProductionThirdStageContributionData() : base(SPYGLASS_THIRD_STAGE_CONTRIBUTION_LIMIT,
        THIRD_STAGE_NAME)
    {
    }
}