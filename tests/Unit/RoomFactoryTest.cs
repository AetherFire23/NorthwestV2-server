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

    [Theory]
    [ClassData(typeof(RoomConnectionsTestData))]
    public void GivenFactory_WhenCreatingRooms_ThenAllAdjacentRoomsAreInRoom(RoomEnum room,
        IEnumerable<RoomEnum> adjacents)
    {
        Game fakeGame = new Game();
        RoomFactory roomFactory = new RoomFactory();

        IEnumerable<Room> rooms = roomFactory.CreateRoomsForGame(fakeGame);

        Room testedRoom = rooms.First(x => x.RoomEnum == room);
        IEnumerable<RoomEnum> adjacentsAsEnum = testedRoom.AdjacentRooms.Select(x => x.RoomEnum);
        Assert.True(adjacents.ToHashSet().SetEquals(adjacentsAsEnum));
    }

    [Fact]
    public void GivenFactory_WhenCreatingRooms_ThenEveryConnectionIsTwoWay()
    {
        Game fakeGame = new Game();
        RoomFactory roomFactory = new RoomFactory();

        List<Room> rooms = roomFactory.CreateRoomsForGame(fakeGame).ToList();

        foreach (Room room in rooms)
        {
            foreach (Room adjacent in room.AdjacentRooms)
            {
                Assert.Contains(room, adjacent.AdjacentRooms);
            }
        }
    }
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
            RoomEnum.CrowsNest,
            RoomEnum.ForeCastle,
            RoomEnum.QuarterDeck,
            RoomEnum.ChartsRoom,
            RoomEnum.FrontStairway,
            RoomEnum.RearStairway
        ]);
        this.Add(RoomEnum.ForeCastle, [RoomEnum.MainDeck]);
        this.Add(RoomEnum.QuarterDeck, [RoomEnum.MainDeck]);
        this.Add(RoomEnum.ChartsRoom, [RoomEnum.MainDeck, RoomEnum.FrontStairway]);
        this.Add(RoomEnum.CaptainsQuarters, [RoomEnum.RearStairway]);
        this.Add(RoomEnum.FrontStairway, [RoomEnum.MainDeck, RoomEnum.ChartsRoom, RoomEnum.Mess]);
        this.Add(RoomEnum.RearStairway, [RoomEnum.MainDeck, RoomEnum.CaptainsQuarters, RoomEnum.MiddleCorridor]);
        this.Add(RoomEnum.Sickbay, [RoomEnum.Brig, RoomEnum.Mess]);
        this.Add(RoomEnum.Brig, [RoomEnum.Sickbay]);
        this.Add(RoomEnum.Mess, [
            RoomEnum.FrontStairway,
            RoomEnum.Sickbay,
            RoomEnum.Galley,
            RoomEnum.CrewsQuarters,
            RoomEnum.MiddleCorridor
        ]);
        this.Add(RoomEnum.CrewsQuarters, [RoomEnum.Mess]);
        this.Add(RoomEnum.Magazine, [RoomEnum.MiddleCorridor]);
        this.Add(RoomEnum.Galley, [RoomEnum.Mess, RoomEnum.MiddleCorridor]);
        this.Add(RoomEnum.MiddleCorridor, [
            RoomEnum.RearStairway,
            RoomEnum.Mess,
            RoomEnum.Galley,
            RoomEnum.Magazine,
            RoomEnum.OfficersQuarters,
            RoomEnum.LaundryRoom,
            RoomEnum.LowerStairway
        ]);
        this.Add(RoomEnum.OfficersQuarters, [RoomEnum.MiddleCorridor]);
        this.Add(RoomEnum.LaundryRoom, [RoomEnum.MiddleCorridor]);
        this.Add(RoomEnum.LowerStairway, [RoomEnum.MiddleCorridor, RoomEnum.LowerCorridor]);
        this.Add(RoomEnum.LowerCorridor, [
            RoomEnum.LowerStairway,
            RoomEnum.Workshop,
            RoomEnum.Armory,
            RoomEnum.EngineRoom,
            RoomEnum.BoilerRoom,
            RoomEnum.Hold
        ]);
        this.Add(RoomEnum.Workshop, [RoomEnum.LowerCorridor]);
        this.Add(RoomEnum.Armory, [RoomEnum.LowerCorridor]);
        this.Add(RoomEnum.EngineRoom, [RoomEnum.LowerCorridor]);
        this.Add(RoomEnum.BoilerRoom, [RoomEnum.LowerCorridor]);
        this.Add(RoomEnum.Hold, [RoomEnum.LowerCorridor, RoomEnum.Coal]);
        this.Add(RoomEnum.Coal, [RoomEnum.Hold, RoomEnum.Food]);
        this.Add(RoomEnum.Food, [RoomEnum.Coal]);
    }
}
