using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

public class UnfinishedSpyglass : ProductionItemBase
{
    // public StageBase CurrentStage { get; set; }

    public UnfinishedSpyglass() : base(ItemTypes.UnfinishedSpyglass, 1)
    {
        // this.CurrentStage = stage;
    }

    public static UnfinishedSpyglass CreateFromItemsAndLock(Scrap scrap)
    {
        SpyglassFirstStageData spyglassFirstStage = new SpyglassFirstStageData();

        UnfinishedSpyglass unfinishedSpyglass = new UnfinishedSpyglass();

        unfinishedSpyglass.LockForProduction(scrap);
        
        return unfinishedSpyglass;
    }

    public override string ToString()
    {
        return nameof(UnfinishedSpyglass);
    }
}