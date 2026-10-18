using Microsoft.Extensions.Logging;
using NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.Domain;

//TODO: Maybe rename to InitialzePlayersForgameService
public class PlayerFactory
{
    private readonly IRandomProvider _randomProvider;
    private readonly IEnumerable<RoleInitializer> _roleInitializers;
    private readonly ILogger<PlayerFactory> _logger;

    public PlayerFactory(IRandomProvider randomProvider, IEnumerable<RoleInitializer> roleInitializers,
        ILogger<PlayerFactory> logger)
    {
        _randomProvider = randomProvider;
        _roleInitializers = roleInitializers;
        _logger = logger;
    }

    /// <summary>
    /// Creates the fresh players for the game.  
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Player> CreateFreshPlayersForGame(List<User> users, Game game, IEnumerable<Room> rooms)
    {
        // Shuffles the roles and assigns the role to each player while creating the player. 
        // I must to this at the very end I think, using a for() loop because I also need to put some dependencies 
        List<Roles> allShuffledRoles = GetShuffledRoles(users);

        List<Player> players = CreateInitializedPlayers(users, game, rooms, allShuffledRoles);

        return players;
    }

    /// <summary>
    /// Creates and initializes all <see cref="Player"/> instances for a game,
    /// assigning each user a role and invoking the appropriate role initializer.
    /// </summary>
    /// <param name="users">
    /// The list of users participating in the game. Each user will be mapped to a role
    /// based on their index in the list.
    /// </param>
    /// <param name="game">
    /// The game instance for which players are being created.
    /// </param>
    /// <param name="rooms">
    /// The collection of rooms available in the game. These are passed to each role
    /// initializer so roles can configure room‑specific state if needed.
    /// </param>
    /// <param name="allShuffledRoles">
    /// A shuffled list of roles, one for each user. The index of each role corresponds
    /// to the index of the user it will be assigned to.
    /// </param>
    /// <returns>
    /// A fully initialized list of <see cref="Player"/> objects, one for each user,
    /// created through the appropriate <see cref="RoleInitializer"/>.
    /// </returns>
    /// <remarks>
    /// This method orchestrates player creation but delegates all role‑specific
    /// initialization to the corresponding <see cref="RoleInitializer"/>.  
    /// It assumes:
    /// <list type="bullet">
    /// <item>The number of users matches the number of roles.</item>
    /// <item>Each role has a registered <see cref="RoleInitializer"/>.</item>
    /// </list>
    /// </remarks>
    private List<Player> CreateInitializedPlayers(List<User> users, Game game, IEnumerable<Room> rooms,
        List<Roles> allShuffledRoles)
    {
        // Prepare the list that will hold all created Player instances.
        List<Player> players = new();

        // Iterate through each user by index so we can map them to the role at the same index.
        for (var i = 0; i < users.ToList().Count; i++)
        {
            User user = users[i];

            // Retrieve the role assigned to this user (roles are pre-shuffled).
            Roles role = allShuffledRoles[i];

            /*
             Find the RoleInitializer responsible for creating players of this role.
             Assumes exactly one initializer exists for each role.
             */
            RoleInitializer roleInitializer = _roleInitializers.First(x => x.Role == role);

            // Build the context object that provides all necessary data
            // for the role initializer to construct and configure the player.
            RoleInitializationContext roleInitializationContext = new RoleInitializationContext
            {
                Game = game,
                Rooms = rooms.ToList(),
                User = user
            };

            Player createdPlayer = roleInitializer.CreateAndInitializePlayer(roleInitializationContext);
            players.Add(createdPlayer);
        }

        return players;
    }

    private List<Roles> GetShuffledRoles(List<User> users)
    {
        List<Roles> allShuffledRoles = _randomProvider.Shuffle(Enum.GetValues<Roles>()).ToList();
        //
        if (users.Count() != allShuffledRoles.Count())
        {
            throw new ArgumentException(
                "Must have as many users as roles in a game ");
        }

        return allShuffledRoles;
    }
}