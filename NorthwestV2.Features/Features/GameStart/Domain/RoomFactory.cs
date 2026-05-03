using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.Domain;

public class RoomFactory
{
    // TODO: Write test that checks if amount of rooms is equal to the amount of game rooms that should exist
    public List<Room> CreateRoomsForGame(Game game)
    {
        Room boilerRoom = new Room
        {
            RoomEnum = RoomEnum.BoilerRoom,
            Game = game,
            Inventory = new Inventory(),
        };

        // boilerRoom.Inventory.Items.Add(new Item(ItemTypes.Spyglass, 2)
        // {
        //     ItemType = ItemTypes.BrittleSword,
        //     CarryValue = 1,
        // });

        Room brig = new Room
        {
            RoomEnum = RoomEnum.Brig,
            Game = game,
            Inventory = new Inventory(),
        };

        Room armory = new Room
        {
            RoomEnum = RoomEnum.Armory,
            Game = game,
            Inventory = new Inventory(),
        };

        Room captainsQuarters = new Room
        {
            RoomEnum = RoomEnum.CaptainsQuarters,
            Game = game,
            Inventory = new Inventory(),
        };

        Room chartsRoom = new Room
        {
            RoomEnum = RoomEnum.ChartsRoom,
            Game = game,
            Inventory = new Inventory(),
        };

        Room coal = new Room
        {
            RoomEnum = RoomEnum.Coal,
            Game = game,
            Inventory = new Inventory(),
        };

        Room crowsNest = new Room
        {
            RoomEnum = RoomEnum.CrowsNest,
            Game = game,
            Inventory = new Inventory(),
        };

        Room crewsQuarters = new Room
        {
            RoomEnum = RoomEnum.CrewsQuarters,
            Game = game,
            Inventory = new Inventory(),
        };

        Room engineRoom = new Room
        {
            RoomEnum = RoomEnum.EngineRoom,
            Game = game,
            Inventory = new Inventory(),
        };

        Room food = new Room
        {
            RoomEnum = RoomEnum.Food,
            Game = game,
            Inventory = new Inventory(),
        };

        Room forecastle = new Room
        {
            RoomEnum = RoomEnum.ForeCastle,
            Game = game,
            Inventory = new Inventory(),
        };

        Room frontStairway = new Room
        {
            RoomEnum = RoomEnum.FrontStairway,
            Game = game,
            Inventory = new Inventory(),
        };

        Room workshop = new Room
        {
            RoomEnum = RoomEnum.Workshop,
            Game = game,
            Inventory = new Inventory(),
        };

        Room sickbay = new Room
        {
            RoomEnum = RoomEnum.Sickbay,
            Game = game,
            Inventory = new Inventory(),
        };

        Room middleCorridor = new Room
        {
            RoomEnum = RoomEnum.MiddleCorridor,
            Game = game,
            Inventory = new Inventory(),
        };

        Room officersQuarters = new Room
        {
            RoomEnum = RoomEnum.OfficersQuarters,
            Game = game,
            Inventory = new Inventory(),
        };

        Room hold = new Room
        {
            RoomEnum = RoomEnum.Hold,
            Game = game,
            Inventory = new Inventory(),
        };

        Room magazine = new Room
        {
            RoomEnum = RoomEnum.Magazine,
            Game = game,
            Inventory = new Inventory(),
        };

        Room mainDeck = new Room
        {
            RoomEnum = RoomEnum.MainDeck,
            Game = game,
            Inventory = new Inventory(),
        };

        Room mess = new Room
        {
            RoomEnum = RoomEnum.Mess,
            Game = game,
            Inventory = new Inventory(),
        };

        Room laundryRoom = new Room
        {
            RoomEnum = RoomEnum.LaundryRoom,
            Game = game,
            Inventory = new Inventory(),
        };
        Room galley = new Room
        {
            RoomEnum = RoomEnum.Galley,
            Game = game,
            Inventory = new Inventory(),
        };
        Room lowerCorridor = new Room
        {
            RoomEnum = RoomEnum.LowerCorridor,
            Game = game,
            Inventory = new Inventory(),
        };

        Room lowerStairway = new Room
        {
            RoomEnum = RoomEnum.LowerStairway,
            Game = game,
            Inventory = new Inventory(),
        };
        Room quarterDeck = new Room
        {
            RoomEnum = RoomEnum.QuarterDeck,
            Game = game,
            Inventory = new Inventory(),
        };

        Room rearStairway = new Room
        {
            RoomEnum = RoomEnum.RearStairway,
            Game = game,
            Inventory = new Inventory(),
        };

        // /*
        //  *  CROWS NEST
        //  */
        // mainDeck.AdjacentRooms.Add(crowsNest);
        // crowsNest.AdjacentRooms.Add(mainDeck);
        //
        //
        //
        // // MAIN DECK
        // mainDeck.AdjacentRooms.Add(forecastle);
        // forecastle.AdjacentRooms.Add(mainDeck);
        // //
        // mainDeck.AdjacentRooms.Add(frontStairway);
        // frontStairway.AdjacentRooms.Add(mainDeck);
        //
        // mainDeck.AdjacentRooms.Add(quarterDeck);
        // quarterDeck.AdjacentRooms.Add(mainDeck);
        //
        // mainDeck.AdjacentRooms.Add(rearStairway);
        // rearStairway.AdjacentRooms.Add(mainDeck);
        // //
        // /*
        //  *  CROWS NEST
        //  */
        //
        //
        //
        // /*
        //  * 3RD ROOM
        //  */
        //
        // // LOWER CORRIDOR
        // lowerCorridor.AdjacentRooms.Add(workshop);
        // workshop.AdjacentRooms.Add(lowerCorridor);
        //
        // lowerCorridor.AdjacentRooms.Add(boilerRoom);
        // boilerRoom.AdjacentRooms.Add(lowerCorridor);


        // NOT REAL CONNECTION

        //
        List<Room> rooms = new([
            brig,
            boilerRoom,
            workshop,
            coal,
            frontStairway,
            forecastle,
            food,
            engineRoom,
            chartsRoom,
            captainsQuarters,
            armory,
            crewsQuarters,
            sickbay,
            middleCorridor,
            crowsNest,
            hold,
            officersQuarters,
            mess,
            mainDeck,
            magazine,
            laundryRoom,
            galley,
            lowerCorridor,
            lowerStairway,
            quarterDeck,
            rearStairway
        ]);

        return rooms;
    }

    public void ConnectRooms()
    {
    }
}

public class ConnectRoomContext
{
    public Room Brig { get; set; }
    public Room BoilerRoom { get; set; }
    public Room Workshop { get; set; }
    public Room Coal { get; set; }
    public Room FrontStairway { get; set; }
    public Room Forecastle { get; set; }
    public Room Food { get; set; }
    public Room EngineRoom { get; set; }
    public Room ChartsRoom { get; set; }
    public Room CaptainsQuarters { get; set; }
    public Room Armory { get; set; }
    public Room CrewsQuarters { get; set; }
    public Room Sickbay { get; set; }
    public Room MiddleCorridor { get; set; }
    public Room CrowsNest { get; set; }
    public Room Hold { get; set; }
    public Room OfficersQuarters { get; set; }
    public Room Mess { get; set; }
    public Room MainDeck { get; set; }
    public Room Magazine { get; set; }
    public Room LaundryRoom { get; set; }
    public Room Galley { get; set; }
    public Room LowerCorridor { get; set; }
    public Room LowerStairway { get; set; }
    public Room QuarterDeck { get; set; }
    public Room RearStairway { get; set; }

    private ConnectRoomContext()
    {
    }

    public static ConnectRoomContext FromRooms(IEnumerable<Room> rooms)
    {
        var roomsList = rooms.ToList();
        return new ConnectRoomContext
        {
            Brig = roomsList.First(x => x.RoomEnum == RoomEnum.Brig),
            BoilerRoom = roomsList.First(x => x.RoomEnum == RoomEnum.BoilerRoom),
            Workshop = roomsList.First(x => x.RoomEnum == RoomEnum.Workshop),
            Coal = roomsList.First(x => x.RoomEnum == RoomEnum.Coal),
            FrontStairway = roomsList.First(x => x.RoomEnum == RoomEnum.FrontStairway),
            Forecastle = roomsList.First(x => x.RoomEnum == RoomEnum.ForeCastle),
            Food = roomsList.First(x => x.RoomEnum == RoomEnum.Food),
            EngineRoom = roomsList.First(x => x.RoomEnum == RoomEnum.EngineRoom),
            ChartsRoom = roomsList.First(x => x.RoomEnum == RoomEnum.ChartsRoom),
            CaptainsQuarters = roomsList.First(x => x.RoomEnum == RoomEnum.CaptainsQuarters),
            Armory = roomsList.First(x => x.RoomEnum == RoomEnum.Armory),
            CrewsQuarters = roomsList.First(x => x.RoomEnum == RoomEnum.CrewsQuarters),
            Sickbay = roomsList.First(x => x.RoomEnum == RoomEnum.Sickbay),
            MiddleCorridor = roomsList.First(x => x.RoomEnum == RoomEnum.MiddleCorridor),
            CrowsNest = roomsList.First(x => x.RoomEnum == RoomEnum.CrowsNest),
            Hold = roomsList.First(x => x.RoomEnum == RoomEnum.Hold),
            OfficersQuarters = roomsList.First(x => x.RoomEnum == RoomEnum.OfficersQuarters),
            Mess = roomsList.First(x => x.RoomEnum == RoomEnum.Mess),
            MainDeck = roomsList.First(x => x.RoomEnum == RoomEnum.MainDeck),
            Magazine = roomsList.First(x => x.RoomEnum == RoomEnum.Magazine),
            LaundryRoom = roomsList.First(x => x.RoomEnum == RoomEnum.LaundryRoom),
            Galley = roomsList.First(x => x.RoomEnum == RoomEnum.Galley),
            LowerCorridor = roomsList.First(x => x.RoomEnum == RoomEnum.LowerCorridor),
            LowerStairway = roomsList.First(x => x.RoomEnum == RoomEnum.LowerStairway),
            QuarterDeck = roomsList.First(x => x.RoomEnum == RoomEnum.QuarterDeck),
            RearStairway = roomsList.First(x => x.RoomEnum == RoomEnum.RearStairway)
        };
    }
}