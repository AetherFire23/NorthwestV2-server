using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

public class RoomHasItemRequirement : ActionRequirement
{
    private readonly Room _room;
    private readonly List<ItemTypes> _requiredItems;

    public RoomHasItemRequirement(Room room, List<ItemTypes> requiredItems)
    {
        _room = room;
        _requiredItems = requiredItems;
    }

    public RoomHasItemRequirement(Room room, ItemTypes requiredItems) : this(room, [requiredItems])
    {
    }

    public override string Description => $"Player must have this or these items : {String.Join(",", _requiredItems)}";

    public override bool IsFulfilled
    {
        get
        {
            var playerItemTypes = _room.Inventory.Items.Select(x => x.ItemType);
            bool isContainedAll = _requiredItems.All(playerItemTypes.Contains);
            return isContainedAll;
        }
    }
}