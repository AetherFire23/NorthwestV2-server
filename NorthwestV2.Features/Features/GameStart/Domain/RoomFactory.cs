using System.Data.Common;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.Domain;

public class RoomFactory
{
    // TODO: Write test that checks if amount of rooms is equal to the amount of game rooms that should exist
    public IEnumerable<Room> CreateRoomsForGame(Game game)
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

        /*
         *  CROWS NEST
         */
        ConnectRooms(mainDeck, crowsNest);

        /*
         *  3RD FLOOR
         */
        ConnectRooms(mainDeck, forecastle);
        ConnectRooms(mainDeck, quarterDeck);
        ConnectRooms(mainDeck, rearStairway);
        ConnectRooms(mainDeck, frontStairway);

        ConnectRooms(chartsRoom, frontStairway);

        ConnectRooms(captainsQuarters, rearStairway);

        /*
         * 2ND FLOOR
         */

        ConnectRooms(rearStairway, mess);
        ConnectRooms(mess, sickbay);
        ConnectRooms(sickbay, brig);
        ConnectRooms(mess, crewsQuarters);
        ConnectRooms(mess, galley);
        ConnectRooms(mess, middleCorridor);

        ConnectRooms(middleCorridor, magazine);
        ConnectRooms(middleCorridor, officersQuarters);
        ConnectRooms(middleCorridor, laundryRoom);
        ConnectRooms(middleCorridor, lowerStairway);
        ConnectRooms(middleCorridor, rearStairway);


        /*
         * 1ND FLOOR
         */
        ConnectRooms(lowerStairway, lowerCorridor);

        ConnectRooms(lowerCorridor, armory);
        ConnectRooms(lowerCorridor, workshop);
        ConnectRooms(lowerCorridor, engineRoom);
        ConnectRooms(lowerCorridor, boilerRoom);
        ConnectRooms(lowerCorridor, hold);

        ConnectRooms(hold, coal);
        ConnectRooms(hold, food);

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

    private void ConnectRooms(Room r1, Room r2)
    {
        r1.AdjacentRooms.Add(r2);
        r2.AdjacentRooms.Add(r1);
    }
}