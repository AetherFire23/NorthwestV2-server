using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;

namespace ERP.Testing.Domain;

[TestSubject(typeof(RoomFactory))]
public class RoomFactoryTest
{
    [Fact]
    public void GivenRoomFactory_WhenCreateRooms_ThenAllRoomsAreCreated()
    {
        Game fakeGame = new Game();
        RoomFactory roomFactory = new RoomFactory();

        IEnumerable<Room> rooms = roomFactory.CreateRoomsForGame(fakeGame);

        Assert.True(rooms.Count() == 2);
    }
}