using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

public class ProductionItemBase : ItemBase
{
    public List<ItemBase> LockedItems { get; set; }
    // TODO: Maybe move 


    public ProductionItemBase(ItemTypes itemType, int carryValue) : base(itemType, carryValue)
    {
    }

    public void LockForProduction(NormalItemBase other)
    {
        if (other.IsLocked)
        {
            throw new Exception($"Cannot be locked twice - {other}");
        }

        other.IsLocked = true;
        other.ProductionItem = this;
    }
}