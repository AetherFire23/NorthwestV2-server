using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Dummies;

public static class TestPlayers
{
    public static Game CreateTestGame()
    {
        var game = new Game();
        return game;
    }


    public static Room CreateTestRoom(RoomEnum roomEnum, Game game)
    {
        Room room = new Room()
        {
            Id = Guid.NewGuid(),
            Game = game,
            RoomEnum = roomEnum,
            Inventory = new Inventory(),
        };

        return room;
    }

    public static Player CreateTestPlayer(int toughness, Roles role, Room initialRoom, Game game)
    {
        Player player = new Player()
        {
            Id = Guid.NewGuid(),
            Game = game,
            Room = initialRoom,
            Role = role,
            User = new User()
            {
                HashedPassword = "noneed",
                Username = "toSpeciyfy"
            },
            BaseToughness = toughness,
        };

        return player;
    }
}