using System.Text.Json.Serialization;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._3_Third;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages.Stages;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

/// <summary>
/// Each production is divided into one or several stages; each with its own TP quota and room requirement.
/// Contributions are permanent, but bound to the ProductionItem.
/// When one stage is complete, the next becomes immediately available.
///
/// Also, it's a record. The reason is that it's getting saved inside of entity framework.
/// I need immutable updates for records. Ef core checks only reference equality for 
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SpyglassFirstStageContributionData), "spyglass_first_stage")]
[JsonDerivedType(typeof(SpyglassSecondStageContributionData), "spyglass_second_stage")]
[JsonDerivedType(typeof(SpyglassProductionThirdStageContributionData), "spyglass_third_stage")]
public abstract record StageContributionBase
{
    public string StageName { get; set; }
    public int Contributions { get; set; }
    public bool IsStageEnded => Contributions >= End;
    public bool IsProductionComplete => this.Contributions == End && GetNextStage() is null;

    [JsonConstructor]
    public StageContributionBase(string stageName, int contributions, int end)
    {
        StageName = stageName;
        Contributions = contributions;
        End = end;
    }

    /// <summary>
    /// The required time points to end the current stage. 
    /// </summary>
    public int End { get; set; }

    /// <summary>
    /// The role required to contribute at reduced cost (1 TP). Non-specialists pay 3 TP, QuarterMaster pays 2 TP.
    /// </summary>
    public virtual Roles? RequiredRole => null;

    public int CalculateCostForContribution(Player player)
    {
        if (player.Role == RequiredRole)
            return 1;

        if (player.Role == Roles.QuarterMaster)
            return 2;

        return 3;
    }

    public StageContributionBase(int end, string stageName)
    {
        End = end;
        StageName = stageName;
    }

    public StageContributionBase GetNextStageIfStageEnded()
    {
        if (!IsStageEnded)
        {
            throw new Exception("Can only advance to the next stage if the current stage is over.");
        }

        StageContributionBase nextStageContribution = GetNextStage();

        if (nextStageContribution is null)
        {
            throw new Exception("This stage doesn't have a next stage");
        }

        return nextStageContribution;
    }

    protected virtual StageContributionBase? GetNextStage()
    {
        return null;
    }

    public abstract List<ActionRequirement> GetRequirements(Player player);


}