using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameStart.RoleInitializations;
using AetherFire23.ERP.Domain.GameStart.RoleInitializations.PlayerInitializers;
using AetherFire23.ERP.Domain.Role;

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