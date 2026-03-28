using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._3_Third;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._2_Second;

public record SpyglassSecondStageData : StageBase
{
    public SpyglassSecondStageData() : base(12, "spyglass Second stage")
    {
    }

    protected override StageBase? GetNextStage()
    {
        return new SpyglassProductionThirdStageData();
    }
}