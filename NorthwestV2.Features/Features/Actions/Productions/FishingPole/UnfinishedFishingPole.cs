using NorthwestV2.Features.Features.Actions.Productions.Core.Entities;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.FishingPole;

public class UnfinishedFishingPole : ProductionItemBase
{
    public UnfinishedFishingPole() : base(ItemTypes.UnfinishedFishingPole, 1, new FishingPoleStage())
    {
    }

    protected override CommonItemBase CreateFinishedItem(Player player)
    {
        FishingPole fishingPole = new FishingPole();

        return fishingPole;
    }
}