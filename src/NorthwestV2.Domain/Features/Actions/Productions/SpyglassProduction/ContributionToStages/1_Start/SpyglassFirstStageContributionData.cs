using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._2_Second;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._1_Start;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="SpyglassFirstStageContributionData"/> class
    /// with the contribution limit and descriptive stage name.
    /// </summary>
    public SpyglassFirstStageContributionData() : base(SPYGLASS_FIRST_STAGE_CONTRIBUTION_LIMIT,
        "First stage - assembly of casings and lenses")
    {
    }

    protected override StageContributionBase? GetNextStage()
    {
        return new SpyglassSecondStageContributionData();
    }
}