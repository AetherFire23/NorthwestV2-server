using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

public class ProductionItem : Item
{
    public List<Item> LockedItems { get; set; }

    public ProductionItem(ItemTypes itemType, int carryValue) : base(itemType, carryValue)
    {
    }

    public void LockForProduction(Item other)
    {
        other.IsLocked = true;
        other.ProductionItem = this;
    }
}