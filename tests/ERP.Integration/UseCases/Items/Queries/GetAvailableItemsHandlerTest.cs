using JetBrains.Annotations;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Items.Queries;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Items.Queries;

[TestSubject(typeof(GetAvailableItemsHandler))]
public class GetAvailableItemsHandlerTest : TestBase2, IAsyncLifetime
{
    private GameDataSeed _gameDataSeed;

    public GetAvailableItemsHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    public async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);

        // Give the player some item
        Guid playerId = _gameDataSeed.PlayerIds.First();

        Player player = this.Context.Players.First(x => x.Id == playerId);

        player.Inventory.Items.Add(new Scrap());
        
        player.Room.Inventory.Items.Add(new Scrap());

        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task GivenPlayerInRoom_WhenGetAvailableItems_ThenHisItemsArePresent()
    {
        // TODO: Make player dto !
        Guid playerId = _gameDataSeed.PlayerIds.First();

        GetAvailableItemsResponse getAvailableItems = await Mediator.Send(new GetAvailableItemsRequest()
        {
            PlayerId = playerId,
        });

        Assert.True(getAvailableItems.PlayerItems.Any());
    }

    [Fact]
    public async Task GivenPlayerInRoom_WithRoomWithItems_ThenRoomItemsArePresent()
    {
        // TODO: Make player dto !
        Guid playerId = _gameDataSeed.PlayerIds.First();

        GetAvailableItemsResponse getAvailableItems = await Mediator.Send(new GetAvailableItemsRequest
        {
            PlayerId = playerId,
        });

        Assert.True(getAvailableItems.RoomItems.Any());
    }
}