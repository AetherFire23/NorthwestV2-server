using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameInitialization;
using Mediator;
using NorthwestV2.Practical;
using NorthwestV2.Application.EfCoreExtensions;

namespace NorthwestV2.Application.UseCases.GameStart;

public class CreateGameHandler : IRequestHandler<CreateGameRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;
    private readonly PlayerFactory _playerFactory;
    private readonly RoomFactory _roomFactory;

    public CreateGameHandler(NorthwestContext northwestContext, PlayerFactory playerFactory, RoomFactory roomFactory)
    {
        _northwestContext = northwestContext;
        _playerFactory = playerFactory;
        _roomFactory = roomFactory;
    }

    public async ValueTask<Guid> Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await _northwestContext.Set<User>().FindAllById(request.UserIds);

        // Rooms will be automatically filled by ef core on savechangesasnyc 
        Game game = new Game();


        IEnumerable<Room> rooms = _roomFactory.CreateRoomsForGame(game);
        _northwestContext.Games.Add(game);
        await _northwestContext.SaveChangesAsync();


        // Do a repository for this !
        // I just wanted to keep my domain clean. 
        // So I detach-reattach the adjacentROoms before ef core saves them.
        // It sucks, but keeps my domain perfect 
        Dictionary<Room, List<Room>> roomsToAdjacents = new Dictionary<Room, List<Room>>();
        foreach (Room room in rooms)
        {
            roomsToAdjacents.Add(room, new List<Room>(room.AdjacentRooms));
            room.AdjacentRooms.Clear();

            _northwestContext.Rooms.Add(room);
        }

        await _northwestContext.SaveChangesAsync();

        foreach (var roomsToAdjacent in roomsToAdjacents)
        {
            foreach (Room room in roomsToAdjacent.Value)
            {
                roomsToAdjacent.Key.AdjacentRooms.Add(room);
            }
        }

        // Must add saveChangesAsync first because else it will try to create all entities in AdjacentRooms.  

        // _roomFactory.AssignConnectionsToRooms(rooms);

        IEnumerable<Player> players = _playerFactory.CreateFreshPlayersForGame(users.ToList(), game, rooms);


        // TODO: Create Rooms

        // Room as builder patterns ?
        // No, room as simple objects where I just do Add() to whichever 
        // So it would be a many-to-many
        // Each 
        await _northwestContext.SaveChangesAsync();

        // TODO: Create Items 

        // TODO: Create game and add Players, users, and items, and rooms 


        return game.Id;
    }

    private async Task<IEnumerable<User>> GetUsers(List<Guid> userIds)
    {
        List<User> users = new List<User>();

        foreach (Guid userId in userIds)
        {
            User user = await _northwestContext.Users.FindAsync(userId) ?? throw new Exception("Cannot find this user");
            users.Add(user);
        }

        return users;
    }
}