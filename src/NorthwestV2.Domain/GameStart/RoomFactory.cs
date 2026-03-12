using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.GameStart;

public class RoomFactory
{
    // TODO: Write test that checks if amount of rooms is equal to the amount of game rooms that should exist
    public IEnumerable<Room> CreateRoomsForGame(Game game)
    {
        Room boilerRoom = new Room
        {
            RoomEnum = RoomEnum.BoilerRoom,
            Game = game,
        };
        
        boilerRoom.Inventory.Items.Add(new Item()
        {
            ItemType = ItemTypes.BrittleSword
        });

        Room brig = new Room
        {
            RoomEnum = RoomEnum.Brig,
            Game = game,
        };

        Room armory = new Room
        {
            RoomEnum = RoomEnum.Armory,
            Game = game,
        };

        Room captainsQuarters = new Room
        {
            RoomEnum = RoomEnum.CaptainsQuarters,
            Game = game,
        };

        Room chartsRoom = new Room
        {
            RoomEnum = RoomEnum.ChartsRoom,
            Game = game,
        };

        Room coal = new Room
        {
            RoomEnum = RoomEnum.Coal,
            Game = game,
        };

        Room crowsNest = new Room
        {
            RoomEnum = RoomEnum.CrowsNest,
            Game = game,
        };

        Room crewsQuarters = new Room
        {
            RoomEnum = RoomEnum.CrewsQuarters,
            Game = game,
        };

        Room engineRoom = new Room
        {
            RoomEnum = RoomEnum.EngineRoom,
            Game = game,
        };

        Room food = new Room
        {
            RoomEnum = RoomEnum.Food,
            Game = game,
        };

        Room forecastle = new Room
        {
            RoomEnum = RoomEnum.ForeCastle,
            Game = game,
        };

        Room frontStairway = new Room
        {
            RoomEnum = RoomEnum.FrontStairway,
            Game = game,
        };

        Room workshop = new Room
        {
            RoomEnum = RoomEnum.Workshop,
            Game = game,
        };

        Room sickbay = new Room
        {
            RoomEnum = RoomEnum.Sickbay,
            Game = game,
        };

        Room middleCorridor = new Room
        {
            RoomEnum = RoomEnum.MiddleCorridor,
            Game = game,
        };


        Room officersQuarters = new Room
        {
            RoomEnum = RoomEnum.OfficersQuarters,
            Game = game,
        };

        Room hold = new Room
        {
            RoomEnum = RoomEnum.Hold,
            Game = game,
        };

        Room magazine = new Room
        {
            RoomEnum = RoomEnum.Magazine,
            Game = game,
        };

        Room mainDeck = new Room
        {
            RoomEnum = RoomEnum.MainDeck,
            Game = game,
        };

        Room mess = new Room
        {
            RoomEnum = RoomEnum.Mess,
            Game = game,
        };

        Room laundryRoom = new Room
        {
            RoomEnum = RoomEnum.LaundryRoom,
            Game = game,
        };
        Room galley = new Room
        {
            RoomEnum = RoomEnum.Galley,
            Game = game,
        };
        Room lowerCorridor = new Room
        {
            RoomEnum = RoomEnum.LowerCorridor,
            Game = game,
        };

        Room lowerStairway = new Room
        {
            RoomEnum = RoomEnum.LowerStairway,
            Game = game,
        };
        Room quarterDeck = new Room
        {
            RoomEnum = RoomEnum.QuarterDeck,
            Game = game,
        };

        Room rearStairway = new Room
        {
            RoomEnum = RoomEnum.RearStairway,
            Game = game,
        };

        
        mainDeck.AdjacentRooms.Add(crowsNest);
        crowsNest.AdjacentRooms.Add(mainDeck);

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
}