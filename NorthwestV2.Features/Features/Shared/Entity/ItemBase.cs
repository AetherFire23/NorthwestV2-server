using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;

namespace NorthwestV2.Features.Features.Shared.Entity;

/// <summary>
/// Base class for all items in the game.
/// Items can be carried by players, placed in room inventories, or locked inside productions.
/// Each item has a type (determining what it is) and a carry value 
/// </summary>
public class ItemBase : EntityBase
{

    public Guid? InventoryId { get; set; }

    /// <summary>
    /// The inventory that contains this item (may be a player's inventory or a room's inventory).
    /// </summary>
    public virtual Inventory? Inventory { get; set; }

    /// <summary>
    /// The type of this item (e.g., Hammer, Flour, Key), determining what it is and how it can be used.
    /// </summary>
    public ItemTypes ItemType { get; set; }

    /// <summary>
    /// The weight/size value of this item, used to calculate how much a player can carry.
    /// </summary>
    public int CarryValue { get; set; }

    public ItemBase()
    {
    }

    public ItemBase(ItemTypes itemType, int carryValue)
    {
        ItemType = itemType;
        CarryValue = carryValue;
    }

    /// <summary>
    /// Converts this item to an ActionTarget for use in action selection UI.
    /// </summary>
    /// <returns>An ActionTarget representing this item.</returns>
    public ActionTarget ToTarget()
    {
        ActionTarget actionTarget = new ActionTarget()
        {
            Name = this.ItemType.ToString(),
            TargetId = this.Id,
        };

        return actionTarget;
    }
}