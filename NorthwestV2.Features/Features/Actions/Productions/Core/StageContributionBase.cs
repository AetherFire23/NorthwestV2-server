using System.Text.Json.Serialization;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction;
using NorthwestV2.Features.Features.Actions.Productions.HammerProduction.Contribution.Stages;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.Core;

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
[JsonDerivedType(typeof(HammerProductionFirstStage), "hammer_first_stage")]
[JsonDerivedType(typeof(FishingPoleStage), nameof(FishingPoleStage))]
public abstract record StageContributionBase
{
    public string StageName { get; set; }
    public int RequiredContributions { get; set; }
    public bool IsStageEnded => RequiredContributions >= End;
    public bool IsProductionComplete => this.RequiredContributions == End && GetNextStage() is null;

    /// <summary>
    /// The required time points to end the current stage. 
    /// </summary>
    public int End { get; set; }

    /// <summary>
    /// The role required to contribute at reduced cost (1 TP). Non-specialists pay 3 TP, QuarterMaster pays 2 TP.
    ///  TODO: Verify if all productions have the same requirement for requireroles
    /// </summary>
    public Roles RequiredRole { get; private set; }

    /// <summary> Its the json sconstructor, do not use by default.  </summary>
    [JsonConstructor]
    public StageContributionBase(string stageName, int requiredContributions, int end, Roles requiredRole)
    {
        StageName = stageName;
        RequiredContributions = requiredContributions;
        End = end;
        this.RequiredRole = requiredRole;
    }

    public StageContributionBase(int end, string stageName, Roles requiredRole)
    {
        End = end;
        StageName = stageName;
        this.RequiredRole = requiredRole;
    }


    public int CalculateCostForContribution(Player player)
    {
        if (player.Role == RequiredRole)
            return 1;

        if (player.Role == Roles.QuarterMaster)
            return 2;

        return 3;
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

    /// <summary>
    /// If left at null, it means there is not next stage. The base class's methods will handle that. 
    /// </summary>
    /// <returns></returns>
    protected virtual StageContributionBase? GetNextStage()
    {
        return null;
    }

    public abstract List<ActionRequirement> GetRequirements(Player player);
}