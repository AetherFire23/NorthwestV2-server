using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole;

public class FishingPole : CommonItemBase
{
    public const ItemTypes ITEM_TYPE = ItemTypes.FishingPole;
    public const int CARRY_VALUE = 1;
    public FishingPole() : base(ITEM_TYPE, CARRY_VALUE)
    {
    }
}
