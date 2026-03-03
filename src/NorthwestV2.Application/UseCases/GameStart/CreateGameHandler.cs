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
        // Rooms will be automatically filled by ef core on savechangesasnyc TODO: a test for this i usspoe 
        Game game = new Game();
        _northwestContext.Games.Add(game);
        
        // Using a trick to get to save the entities & the nested properties without ef core crying. 
        IEnumerable<Room> rooms = _roomFactory.CreateRoomsForGame(game);
        await SaveRoomAndAdjacents(rooms);

        IEnumerable<User> users = await _northwestContext.Set<User>().FindAllById(request.UserIds);

        IEnumerable<Player> players = _playerFactory.CreateFreshPlayersForGame(users.ToList(), game, rooms);

      
        await _northwestContext.SaveChangesAsync();

        // TODO: Create Items 

        // TODO: Create game and add Players, users, and items, and rooms 

        return game.Id;
    }

    // Do a repository for this !
    // I just wanted to keep my domain clean. 
    // So I detach-reattach the adjacentROoms before ef core saves them.
    // It sucks, but keeps my domain perfect 
    // TODO: Fun ? do an AddWithNestedProps extension
    private async ValueTask SaveRoomAndAdjacents(IEnumerable<Room> rooms)
    {
        Dictionary<Room, List<Room>> roomsToAdjacents = new Dictionary<Room, List<Room>>();
        foreach (Room room in rooms)
        {
            roomsToAdjacents.Add(room, new(room.AdjacentRooms));

            _northwestContext.Rooms.Add(room);
        }
        
        foreach (Room room in rooms)
        {
            room.AdjacentRooms.Clear();
        }

        // Saving first so that ef core can know about the existing rooms before trying to add cyclically 
        // nested entities. 
        await _northwestContext.SaveChangesAsync();

        foreach (var roomsToAdjacent in roomsToAdjacents)
        {
            roomsToAdjacent.Key.AdjacentRooms.AddRange(roomsToAdjacent.Value);
        }
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