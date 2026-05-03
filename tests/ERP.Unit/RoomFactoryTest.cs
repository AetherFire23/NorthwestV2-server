using System.Xml.Schema;
using JetBrains.Annotations;
using NorthwestV2.Features.Features.GameStart;
using NorthwestV2.Features.Features.GameStart.Domain;
using NorthwestV2.Features.Features.Shared.Entity;

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
    public void GivenFactory_WhenCreatingRooms_AreAllPresent(RoomEnum room)
    {
        Game fakeGame = new Game();
        RoomFactory roomFactory = new RoomFactory();

        IEnumerable<Room> rooms = roomFactory.CreateRoomsForGame(fakeGame);

        Assert.Contains(room, rooms.Select(x => x.RoomEnum));
    }
    //
    // [Theory]
    // [ClassData(typeof(RoomConnectionsTestData))]
    // public void GivenFactory_WhenCreatingRooms_ThenAllAdjacentRoomsAreInRoom(RoomEnum room,
    //     IEnumerable<RoomEnum> adjacents)
    // {
    //     Game fakeGame = new Game();
    //     RoomFactory roomFactory = new RoomFactory();
    //
    //     IEnumerable<Room> rooms = roomFactory.CreateRoomsForGame(fakeGame);
    //
    //     Room testedRoom = rooms.First(x => x.RoomEnum == room);
    //     IEnumerable<RoomEnum> adjacentsAsEnum = testedRoom.AdjacentRooms.Select(x => x.RoomEnum);
    //     Assert.True(adjacents.All(adjacentsAsEnum.Contains));
    // }
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
        this.Add(RoomEnum.CrowsNest, [RoomEnum.MainDeck]);
        this.Add(RoomEnum.MainDeck, [
            RoomEnum.FrontStairway,
            RoomEnum.RearStairway,
            RoomEnum.CrowsNest,
            RoomEnum.QuarterDeck,
            RoomEnum.ForeCastle
        ]);
        this.Add(RoomEnum.ForeCastle, [
            RoomEnum.MainDeck,
        ]);
        this.Add(RoomEnum.FrontStairway, [
            RoomEnum.MainDeck,
            RoomEnum.ChartsRoom,
        ]);
        this.Add(RoomEnum.ChartsRoom, [
            RoomEnum.FrontStairway,
        ]);
        this.Add(RoomEnum.QuarterDeck, [
            RoomEnum.MainDeck,
        ]);
        this.Add(RoomEnum.RearStairway, [
            RoomEnum.MainDeck,
            RoomEnum.CaptainsQuarters
        ]);
        this.Add(RoomEnum.Sickbay, [
            RoomEnum.Brig,
            RoomEnum.Mess
        ]);
        this.Add(RoomEnum.Brig, [
            RoomEnum.Sickbay,
        ]);
        this.Add(RoomEnum.Mess, [
            RoomEnum.Sickbay,
            RoomEnum.CrowsNest,
            RoomEnum.Galley,
            RoomEnum.MiddleCorridor, 
        ]);
        this.Add(RoomEnum.Galley, [
            RoomEnum.Mess,
        ]);
        
        // TODO: Finish Connections
    }
}