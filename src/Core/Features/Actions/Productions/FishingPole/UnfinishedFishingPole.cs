using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole;

public class UnfinishedFishingPole : ProductionItemBase
{
    public const ItemTypes ITEM_TYPE = ItemTypes.UnfinishedFishingPole;
    public const int CARRY_VALUE = 1;

    
    public UnfinishedFishingPole() : base(ITEM_TYPE, CARRY_VALUE, new FishingPoleStage())
    {
    }

    protected override CommonItemBase CreateFinishedItem(Player player)
    {
        FishingPole fishingPole = new FishingPole();

        return fishingPole;
    }
}