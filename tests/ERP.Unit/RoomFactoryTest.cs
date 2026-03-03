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

        Assert.True(rooms.Count() == Enum.GetValues<RoomEnum>().Length);
    }

    [Theory]
    [ClassData(typeof(RoomEnumTestData))]
    public void TestRoomNames(RoomEnum room)
    {
        Game fakeGame = new Game();
        RoomFactory roomFactory = new RoomFactory();

        IEnumerable<Room> rooms = roomFactory.CreateRoomsForGame(fakeGame);

        Assert.Contains(room, rooms.Select(x => x.RoomEnum));
    }

    // TODO: Assert rooms have connections. 
}

public class RoomEnumTestData : TheoryData<RoomEnum>
{
    public RoomEnumTestData()
    {
        foreach (RoomEnum roomEnum in Enum.GetValues<RoomEnum>())
        {
            this.Add(roomEnum);
        }
    }
}

public class RoomConnectionsTestData : TheoryData<RoomEnum, IEnumerable<RoomEnum>>
{
    public RoomConnectionsTestData()
    {
        var bloop = Enum.GetValues<RoomEnum>();

        // foreach (RoomEnum roomEnum in bloop)
        // {
        //     this.Add(roomEnum);
        // }
    }
}