namespace AetherFire23.ERP.Domain.Entity;

/// <summary>
/// BOth a player and a Room can be an Item owner. Introducing the ivnentory makes things so much easier 
/// </summary>
public class Inventory : EntityBase
{
    // public Guid? PlayerId { get; set; }
    // public Player? Player { get; set; }
    //
    // public Guid? RoomId { get; set; }
    // public Room? Room { get; set; }
    public List<ItemBase> Items { get; set; } = [];

    public ItemBase Find(ItemTypes itemType)
    {
        var item = this.Items.First(x => x.ItemType == itemType);

        return item;
    }

    /// <summary>
    /// FInds the first occurence of item matching the subtype.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Find<T>() where T : ItemBase
    {
        var item = this.Items.First(x => x is T);

        return (T)item;
    }

    public ItemBase FindById(Guid id)
    {
        ItemBase first = Items.First(x => x.Id == id);

        return first;
    }

    public void Add(ItemBase itemBase)
    {
        this.Items.Add(itemBase);
    }

    /// <summary>
    /// Removes the item from the other inventory and adds it to the current inventory. 
    /// </summary>
    /// <param name="otherInventory"></param>
    /// <param name="toTake"></param>
    public void TakeOwnership(Inventory otherInventory, ItemBase toTake)
    {
        otherInventory.Items.Remove(toTake);
        this.Items.Add(toTake);
    }

    /// <summary>
    /// Takes items incoming from a list, add them to this current invenotry, then removes them from the list. 
    /// </summary>
    /// <param name="itemsToTake"></param>
    public void TakeOwnership(List<ItemBase> itemsToTake)
    {
        this.Items.AddRange(itemsToTake);
        itemsToTake.Clear();
    }
}