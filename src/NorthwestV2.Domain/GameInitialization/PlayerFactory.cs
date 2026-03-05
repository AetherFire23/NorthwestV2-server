using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameInitialization.RoleInitializations;
using AetherFire23.ERP.Domain.Role;
using Microsoft.Extensions.Logging;

namespace AetherFire23.ERP.Domain.GameInitialization;

//TODO: Maybe rename to InitialzePlayersForgameService
public class PlayerFactory
{
    private readonly IRandomProvider _randomProvider;
    private readonly IEnumerable<RoleInitializer> _roleInitializers;
    private readonly ILogger<PlayerFactory> _logger;

    public PlayerFactory(IRandomProvider randomProvider, IEnumerable<RoleInitializer> roleInitializers, ILogger<PlayerFactory> logger)
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

    private List<Player> CreateInitializedPlayers(List<User> users, Game game, IEnumerable<Room> rooms,
        List<Roles> allShuffledRoles)
    {
        List<Player> players = new List<Player>();

        // map each player to each role 

        for (var i = 0; i < users.ToList().Count; i++)
        {
            User user = users[i];
            Roles role = allShuffledRoles[i];
            
            _logger.LogInformation($"Created : {role.ToString()}");
            
            RoleInitializer roleInitializer = _roleInitializers.First(x => x.Role == role);

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