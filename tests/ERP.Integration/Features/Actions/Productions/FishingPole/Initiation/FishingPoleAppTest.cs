using JetBrains.Annotations;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Contribution;
using NorthwestV2.Features.Features.Actions.Productions.FishingPole.Initiation;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Productions.FishingPole.Initiation;

[TestSubject(typeof(FishingPoleApp))]
public class FishingPoleAppTest : TestBase2
{
    public FishingPoleAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenScrap_WhenInitiating_ThenUnfinishedFishingRodIsInRoomInventory()
    {
    }

    private async Task<Guid> SetupForFishingRodInitiation()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        Guid playerId = await PlayerHelper.TeleportPlayerTo(
            Scope,
            Context,
            gameDataSeed,
            FishingPoleStage.REQUIRED_ROOM);

        Player player = await base.GetServiceFromScope<IPlayerRepository>()
            .GetPlayerAndRoomAndInventoryAndGame(playerId);

        player.Room.Inventory.Items.Add(new Scrap());

        await Context.SaveChangesAsync();
        return playerId;
    }
}