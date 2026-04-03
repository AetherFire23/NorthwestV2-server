using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Entity;

public class Room : EntityBase
{
    public Guid GameId { get; set; }
    public required Game Game { get; set; }
    public required RoomEnum RoomEnum { get; set; }

    public List<Room> AdjacentRooms { get; set; } = [];

    public Inventory Inventory { get; set; }

    //TODO: One day make method to get 

    /// <summary>
    /// Returns true if at least one item of that type is found in the inventory. 
    /// </summary>
    /// <param name="itemTypes"></param>
    /// <returns></returns>
    public bool Has(ItemTypes itemTypes)
    {
        bool hasItem = this.Inventory.Items.Any(x => x.ItemType == itemTypes);
        return hasItem;
    }
    
    public void RemoveItem(ItemBase item)
    {
        this.Inventory.Items.Remove(item);
    }

    /// <summary>
    /// Takes items incoming from a list, add them to this current invenotry, then removes them from the list. 
    /// </summary>
    /// <param name="itemsToTake"></param>
    public void TakeOwnership(List<ItemBase> itemsToTake)
    {
        this.Inventory.Items.AddRange(itemsToTake);
        itemsToTake.Clear();
    }

    public override string ToString()
    {
        return this.RoomEnum.ToString();
    }

    public ActionTarget ToTarget()
    {
        ActionTarget actionTarget = new ActionTarget
        {
            Name = RoomEnum.ToString(),
            TargetId = Id,
        };

        return actionTarget;
    }
}