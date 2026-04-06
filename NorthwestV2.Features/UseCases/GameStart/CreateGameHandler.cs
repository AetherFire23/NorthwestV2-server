using Mediator;
using NorthwestV2.Features.Features.GameStart;
using NorthwestV2.Features.Features.GameStart.Domain;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;

namespace NorthwestV2.Features.UseCases.GameStart;

public class CreateGameHandler : IRequestHandler<CreateGameRequest, Guid>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly PlayerFactory _playerFactory;
    private readonly IPlayerRepository _playerRepository;
    private readonly RoomFactory _roomFactory;
    private readonly IRoomRepository _roomRepository;

    public CreateGameHandler(PlayerFactory playerFactory, RoomFactory roomFactory, IUserRepository userRepository,
        IUnitOfWork unitOfWork, IGameRepository gameRepository, IPlayerRepository playerRepository,
        IRoomRepository roomRepository)
    {
        _playerFactory = playerFactory;
        _roomFactory = roomFactory;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Creates a new game, generates its rooms, loads the participating users,
    /// creates fresh players for the game, and persists all related entities.
    /// </summary>
    /// <param name="request">
    /// The request containing the list of user IDs that will participate in the game.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to observe cancellation requests.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created <see cref="Game"/>.
    /// </returns>
    /// <remarks>
    /// This handler orchestrates the full setup of a new game instance:
    /// <list type="bullet">
    /// <item>Creates the <see cref="Game"/> entity.</item>
    /// <item>Generates rooms and their adjacency graph using the room factory.</item>
    /// <item>Loads all participating users.</item>
    /// <item>Creates fresh <see cref="Player"/> entities for each user.</item>
    /// <item>Persists all entities to the database.</item>
    /// </list>
    /// 
    /// Domain logic (e.g., room generation, player initialization) is delegated to
    /// factories and domain services. This method focuses solely on application-layer
    /// orchestration and persistence.
    /// 
    /// Item creation and final game assembly are intentionally left as TODOs.
    /// </remarks>
    public async ValueTask<Guid> Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        // Rooms will be automatically filled by ef core on savechangesasnyc TODO: a test for this i usspoe 
        Game game = new Game();

        _gameRepository.Add(game);

        // Using a trick to get to save the entities & the nested properties without ef core crying. 
        IEnumerable<Room> rooms = _roomFactory.CreateRoomsForGame(game);


        await _roomRepository.SaveRoomAndAdjacents(rooms);

        IEnumerable<User> usersInGame = await _userRepository.GetAllById(request.UserIds.ToList());

        IEnumerable<Player> players = _playerFactory.CreateFreshPlayersForGame(usersInGame.ToList(), game, rooms);

        _playerRepository.AddRange(players);

        await _unitOfWork.SaveChangesAsync();

        // TODO: Create Items 


        // TODO: Create game and add Players, users, and items, and rooms 

        return game.Id;
    }
}