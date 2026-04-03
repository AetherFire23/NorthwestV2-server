using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.ContributionToStages.Stages;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

public class UnfinishedSpyglass : ProductionItemBase
{
    private UnfinishedSpyglass()
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

    // TODO: Maybe instead just return an ItemBase; and we can Clear the locked items automatically. and add the item to the room's inventory

    public override NormalItemBase CreateFinishedItem(Player player)
    {
        // TODO: Might wanna abstract deletion behaviour after. 
        Spyglass spyglass = new Spyglass();



        return spyglass;
    }
}