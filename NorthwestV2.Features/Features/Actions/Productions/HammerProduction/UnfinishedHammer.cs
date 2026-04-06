using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.HammerProduction;

public class UnfinishedHammer : ProductionItemBase
{
    public const ItemTypes ITEM_TYPE = ItemTypes.UnfinishedHammer;
    public const RoomEnum REQUIRED_ROOM = RoomEnum.Workshop;

    private UnfinishedHammer() : base(ITEM_TYPE, 1, new HammerProductionFirstStage())
    {
    }

    public static UnfinishedHammer CreateFromItemsAndLock(Scrap scrap)
    {
        UnfinishedHammer unfinishedHammer = new UnfinishedHammer();
        unfinishedHammer.LockForProduction(scrap);
        return unfinishedHammer;
    }

    protected override CommonItemBase CreateFinishedItem(Player player)
    {
        return new Hammer();
    }
}