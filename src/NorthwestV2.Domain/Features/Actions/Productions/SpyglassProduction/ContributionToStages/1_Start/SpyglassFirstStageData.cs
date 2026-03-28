using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

public record SpyglassFirstStageData : StageBase
{
    public const int SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT = 8;
    public SpyglassFirstStageData() : base(SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT, "First stage - assembly of casings and lenses")
    {
    }
}