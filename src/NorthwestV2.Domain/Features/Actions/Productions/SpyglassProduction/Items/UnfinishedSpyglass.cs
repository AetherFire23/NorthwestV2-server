using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages._1_Start;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

public class UnfinishedSpyglass : ProductionItemBase
{
    public UnfinishedSpyglass()
    {
    }

    public UnfinishedSpyglass(StageBase initialFirstStage) : base(ItemTypes.UnfinishedSpyglass, 1, initialFirstStage)
    {
    }
    
    /// <summary>
    /// Creates an unfinished spyglass for the production of the Spyglass item. 
    /// </summary>
    /// <param name="scrap">The required item to create an unfinished spyglass</param>
    /// <returns></returns>
    public static UnfinishedSpyglass CreateFromItemsAndLock(Scrap scrap)
    {
        SpyglassFirstStageData spyglassFirstStageData = new SpyglassFirstStageData()
        {
            Contributions = 0,
            End = 8,
            StageName = "first stage "
        };
        
        UnfinishedSpyglass unfinishedSpyglass = new UnfinishedSpyglass(spyglassFirstStageData);

        unfinishedSpyglass.LockForProduction(scrap);
        
        return unfinishedSpyglass;
    }

    public override string ToString()
    {
        return nameof(UnfinishedSpyglass);
    }
}