using JetBrains.Annotations;
using NorthwestV2.Features.ApplicationsStuff.EfCoreExtensions;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Items.Commands;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Items.Commands;

[TestSubject(typeof(SwapItemOwnershipHandler))]
public class SwapItemOwnershipHandlerTest : TestBase2
{
    private Player ANY_PLAYER;

    private GameDataSeed _gameDataSeed;
    private Player _anyPlayer;

    public SwapItemOwnershipHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(this.Mediator, this.Context);
        _anyPlayer = await this.Context.Players.FindById(_gameDataSeed.PlayerIds.First());
    }

    [Fact]
    public async Task GivenItemInRoom_WhenPlayerPicksUpItem_ThenItemIsOwnedByRoom()
    {
        /*
         * Give scrap to player
         */
        Scrap item = new Scrap();
        _anyPlayer.Inventory.Items.Add(item);
        await this.Context.SaveChangesAsync();

        await this.Mediator.Send(new SwapItemOwnershipRequest()
        {
            ItemId = item.Id,
            PlayerId = _anyPlayer.Id
        });

        bool roomHasItem = _anyPlayer.Room.Inventory.HasItem(item);
        Assert.True(roomHasItem);
    }

    [Fact]
    public async Task GivenItemInRoom_WhenPlayerPicksUpItem_ThenItemIsOwnedByPlayer()
    {
        /*
         * Give scrap to room
         */
        Scrap item = new Scrap();
        _anyPlayer.Room.Inventory.Items.Add(item);
        await Context.SaveChangesAsync();

        await Mediator.Send(new SwapItemOwnershipRequest
        {
            ItemId = item.Id,
            PlayerId = _anyPlayer.Id
        });

        bool roomHasItem = _anyPlayer.Inventory.HasItem(item);
        Assert.True(roomHasItem);
    }

    [Fact]
    public async Task GivenItemNotInRoom_WhenPlayerPicksUpItem_ThenThrowsException()
    {
        /*
         * Give scrap item to any player in another room.
         */
        Scrap item = new Scrap();
        // find any other palyer whos' not the current player
        Player otherPlayer = Context.Players.First(x => x.Id != _anyPlayer.Id);
        // teleport the other player to another room. 
        otherPlayer.Room = Context.Rooms.First(x => x.RoomEnum != _anyPlayer.Room.RoomEnum);

        // Give him the item
        otherPlayer.Room.Inventory.Items.Add(item);
        await Context.SaveChangesAsync();

        Action action = () =>
        {
            var unit = Mediator.Send(new SwapItemOwnershipRequest
            {
                ItemId = item.Id,
                PlayerId = _anyPlayer.Id
            }).Result;
        };

        Assert.Throws<Exception>(action);
    }
}