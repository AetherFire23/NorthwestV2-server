using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

namespace AetherFire23.ERP.Domain.Entity;

// TODO: Make it abstract please. 
public class Item : EntityBase
{
    // May not have a production at all times 
    public Guid? InventoryId { get; set; }
    public Inventory? Inventory { get; set; }


    public Guid? ProductionItemId { get; set; }
    public ProductionItem? ProductionItem { get; set; }
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
}