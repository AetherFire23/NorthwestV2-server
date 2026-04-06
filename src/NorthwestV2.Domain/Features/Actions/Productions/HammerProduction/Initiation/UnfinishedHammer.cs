using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.HammerProduction.Initiation;

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