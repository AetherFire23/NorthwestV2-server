using NorthwestV2.Features.Features.Actions.Productions.Core;
using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Contribution.Stages;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;

public class UnfinishedSpyglass : ProductionItemBase
{
    public UnfinishedSpyglass()
    {
    }

    private UnfinishedSpyglass(StageContributionBase initialFirstStageContribution) : base(ItemTypes.UnfinishedSpyglass,
        1, initialFirstStageContribution)
    {
    }

    public bool IsProductionComplete => this.CurrentStageContribution.IsProductionComplete;

    /// <summary>
    /// Creates an unfinished spyglass for the production of the Spyglass item. 
    /// </summary>
    /// <param name="scrap">The required item to create an unfinished spyglass</param>
    /// <returns></returns>
    public static UnfinishedSpyglass CreateFromItemsAndLock(Scrap scrap)
    {
        SpyglassFirstStageContributionData
            spyglassFirstStageContributionData = new SpyglassFirstStageContributionData();

        UnfinishedSpyglass unfinishedSpyglass = new UnfinishedSpyglass(spyglassFirstStageContributionData);

        unfinishedSpyglass.LockForProduction(scrap);

        return unfinishedSpyglass;
    }

    public override string ToString()
    {
        return nameof(UnfinishedSpyglass);
    }

    protected override CommonItemBase CreateFinishedItem(Player player)
    {
        Spyglass spyglass = new Spyglass();
        return spyglass;
    }
}