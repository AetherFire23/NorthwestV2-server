using System.Text.Json.Serialization;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._2_Second;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages._3_Third;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

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
[JsonDerivedType(typeof(SpyglassFirstStageData), "spyglass_first_stage")]
[JsonDerivedType(typeof(SpyglassSecondStageData), "spyglass_second_stage")]
[JsonDerivedType(typeof(SpyglassProductionThirdStageData), "spyglass_third_stage")]
public record StageBase
{
    public string StageName { get; set; }
    public int Contributions { get; set; }

    [JsonConstructor]
    public StageBase(string stageName, int contributions, int end)
    {
        StageName = stageName;
        Contributions = contributions;
        End = end;
    }

    /// <summary>
    /// The required time points to end the current stage. 
    /// </summary>
    public int End { get; set; }

    public StageBase(int end, string stageName)
    {
        End = end;
        StageName = stageName;
    }

    public bool IsStageEnded => Contributions >= End;
    public bool IsProductionComplete => this.Contributions == End && GetNextStage() is null;

    public StageBase GetNextStageIfStageEnded()
    {
        if (!IsStageEnded)
        {
            throw new Exception("Can only advance to the next stage if the current stage is over.");
        }

        StageBase nextStage = GetNextStage();

        if (nextStage is null)
        {
            throw new Exception("This stage doesn't have a next stage");
        }

        return nextStage;
    }

    protected virtual StageBase? GetNextStage()
    {
        return null;
    }
}