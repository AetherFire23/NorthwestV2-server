namespace AetherFire23.ERP.Domain.Entity;

/// <summary>
/// </summary>
public class ItemBase : EntityBase
{
    public Guid? InventoryId { get; set; }
    public Inventory? Inventory { get; set; }

    public ItemTypes ItemType { get; set; }
    public int CarryValue { get; set; }

    public ItemBase()
    {
    }

    public ItemBase(ItemTypes itemType, int carryValue)
    {
        ItemType = itemType;
        CarryValue = carryValue;
    }
}