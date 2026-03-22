using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

public class UnfinishedSpyglass : ProductionItem
{
    public UnfinishedSpyglass() : base(ItemTypes.UnfinishedSpyglass, 1)
    {
    }

    public static UnfinishedSpyglass CreateFromItemsAndLock(Scrap scrap)
    {
        UnfinishedSpyglass unfinishedSpyglass = new UnfinishedSpyglass();

        unfinishedSpyglass.LockForProduction(scrap);

        return unfinishedSpyglass;
    }
}