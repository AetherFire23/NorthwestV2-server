using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Item : EntityBase
{
    // May not have a production at all times 
    public Guid? InventoryId { get; set; }
    public Inventory? Inventory { get; set; }

    public Guid? ProductionId { get; set; }
    public Production? Production { get; private set; }
    public ItemTypes ItemType { get; set; }
    public int CarryValue { get; set; }

    // TODO: Maybe move 
    public int TimePointsContributions { get; set; } = 0;
    public bool IsLocked { get; set; } = false;

    public Item()
    {
    }

    public Item(ItemTypes itemType, int carryValue)
    {
        ItemType = itemType;
        CarryValue = carryValue;
    }

    public void LockForProduction(Production production)
    {
        this.IsLocked = true;
        this.Production = production;
    }
}