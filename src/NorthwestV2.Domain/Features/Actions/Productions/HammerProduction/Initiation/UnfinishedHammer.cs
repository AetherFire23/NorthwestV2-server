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

    public override void OnProductionCompleted(Player player)
    {
    }

    public static UnfinishedHammer CreateFromItemsAndLock(Scrap scrap)
    {
        return new UnfinishedHammer();
    }
}