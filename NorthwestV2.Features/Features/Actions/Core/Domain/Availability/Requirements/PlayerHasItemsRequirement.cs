using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Requirements;

public class PlayerHasItemsRequirement : ActionRequirement
{
    private readonly Player _player;
    private readonly List<ItemTypes> _requiredItems;

    public PlayerHasItemsRequirement(Player player, List<ItemTypes> requiredItems)
    {
        _player = player;
        _requiredItems = requiredItems;
    }

    public PlayerHasItemsRequirement(Player player, ItemTypes requiredItems) : this(player, [requiredItems])
    {
    }

    public override string Description => $"Player must have this or these items : {String.Join(",", _requiredItems)}";

    public override bool IsFulfilled
    {
        get
        {
            var playerItemTypes = _player.Inventory.Items.Select(x => x.ItemType);
            bool areAllRequiredItemsOwnedByPlayer = _requiredItems.All(playerItemTypes.Contains);
            return areAllRequiredItemsOwnedByPlayer;
        }
    }
}