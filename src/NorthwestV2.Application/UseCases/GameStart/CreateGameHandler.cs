using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
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
        _northwestContext.Rooms.AddRange(rooms);
        
        // Must add saveChangesAsync first because else it will try to create all entities in AdjacentRooms.  
        await _northwestContext.SaveChangesAsync();

        _roomFactory.AssignConnectionsToRooms(rooms);

        IEnumerable<Player> players = _playerFactory.CreateFreshPlayersForGame(users.ToList());


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