using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Requirements;

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
            bool isContainedAll = _requiredItems.Any(x => _requiredItems.Contains(x));
            return isContainedAll;
        }
    }
}