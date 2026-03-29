using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._3_Third;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._2_Second;

public record SpyglassSecondStageContributionData : StageContributionBase
{
    public const int SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT = 12;

    public SpyglassSecondStageContributionData() : base(SPYGLASS_SECOND_STAGE_CONTRIBUTION_LIMIT, "spyglass Second stage")
    {
    }

    protected override StageContributionBase? GetNextStage()
    {
        return new SpyglassProductionThirdStageContributionData();
    }

    // public override List<ActionRequirement> GetRequirements(Player player)
    // {
    //     return [];
    // }
}