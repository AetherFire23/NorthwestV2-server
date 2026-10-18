using NorthwestV2.Features.Actions.Cooking.CookRation;
using NorthwestV2.Features.Actions.Cooking.Items;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.Cooking;

public class CookRationActionTest
{
    private readonly CookRationAction _action = new();

    private static Player CreatePlayerWithItems(RoomEnum room, params ItemBase[] items)
    {
        Game game = TestPlayers.CreateTestGame();
        Room playersRoom = TestPlayers.CreateTestRoom(room, game);
        Player player = TestPlayers.CreateTestPlayer(10, Roles.Cook, playersRoom, game);
        player.Inventory = new Inventory();
        player.Inventory.Items.AddRange(items);
        return player;
    }

    [Fact]
    public void GivenCan_WhenDeterminingAvailability_ThenCanExecute()
    {
        Player player = CreatePlayerWithItems(RoomEnum.Mess, new Can());

        InstantActionAvailability? availability = _action.DetermineAvailability(player);

        Assert.True(availability!.CanExecute);
    }

    [Fact]
    public void GivenNoCan_WhenDeterminingAvailability_ThenActionIsShown()
    {
        Player player = CreatePlayerWithItems(RoomEnum.Mess);

        InstantActionAvailability? availability = _action.DetermineAvailability(player);

        Assert.NotNull(availability);
    }

    [Fact]
    public void GivenNoCan_WhenDeterminingAvailability_ThenCannotExecute()
    {
        Player player = CreatePlayerWithItems(RoomEnum.Mess);

        InstantActionAvailability? availability = _action.DetermineAvailability(player);

        Assert.False(availability!.CanExecute);
    }

    [Fact]
    public void GivenTwoCans_WhenExecuting_ThenOneCanRemains()
    {
        Player player = CreatePlayerWithItems(RoomEnum.Galley, new Can(), new Can());

        _action.Execute(player);

        Assert.Equal(1, player.Inventory.Items.Count(x => x.ItemType == ItemTypes.Can));
    }

    [Fact]
    public void GivenTwoCans_WhenExecuting_ThenOneRationIsAdded()
    {
        Player player = CreatePlayerWithItems(RoomEnum.Galley, new Can(), new Can());

        _action.Execute(player);

        Assert.Equal(1, player.Inventory.Items.Count(x => x.ItemType == ItemTypes.Ration));
    }
}
