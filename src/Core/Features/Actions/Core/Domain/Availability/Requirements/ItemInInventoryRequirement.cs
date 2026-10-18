using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;

public class ItemInInventoryRequirement : ActionRequirement
{
    private readonly Player _player;
    private readonly List<ItemTypes> _requiredItems;

    public ItemInInventoryRequirement(Player player, List<ItemTypes> requiredItems)
    {
        _player = player;
        _requiredItems = requiredItems;
    }

    public ItemInInventoryRequirement(Player player, ItemTypes requiredItems) : this(player, [requiredItems])
    {
    }

    public override string Description => $"Player must have this or these items : {String.Join(",", _requiredItems)}";

    public override bool IsFulfilled
    {
        get
        {
            IEnumerable<ItemTypes> playerItemTypes = _player.Inventory.Items.Select(x => x.ItemType);
            bool isContainedAll = _requiredItems.All(playerItemTypes.Contains);
            return isContainedAll;
        }
    }
}