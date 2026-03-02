using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain;

public class RoomFactory
{
    // TODO: Write test that checks if amount of rooms is equal to the amount of game rooms that should exist
    public IEnumerable<Room> CreateRoomsForGame(Game game)
    {
        Room brig = new Room
        {
            RoomEnum = RoomEnum.Brig,
            Game = game,
        };

        Room boilerRoom = new Room
        {
            RoomEnum = RoomEnum.Brig,
            Game = game,
        };

        List<Room> rooms = new List<Room>([brig, boilerRoom]);

        return rooms;
    }
}