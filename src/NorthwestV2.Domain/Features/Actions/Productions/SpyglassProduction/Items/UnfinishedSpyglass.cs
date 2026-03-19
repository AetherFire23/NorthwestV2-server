using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

public class UnfinishedSpyglass : Item
{
    public UnfinishedSpyglass() : base(ItemTypes.UnfinishedSpyglass, 1)
    {
    }
}