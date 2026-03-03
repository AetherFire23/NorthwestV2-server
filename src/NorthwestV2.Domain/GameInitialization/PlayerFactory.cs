using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameInitialization.RoleInitializations;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization;

//TODO: Maybe rename to InitialzePlayersForgameService
public class PlayerFactory
{
    private readonly IRandomProvider _randomProvider;
    private readonly IEnumerable<RoleInitializer> _roleInitializers;

    public PlayerFactory(IRandomProvider randomProvider, IEnumerable<RoleInitializer> roleInitializers)
    {
        _randomProvider = randomProvider;
        _roleInitializers = roleInitializers;
    }

    // TODO: Create a user and give it a role.
    // Roles cant be repeated within a single game. 
    // Implementation idea : Shuffle the list of Roles
    // then use iterator to create a new player.
    /// <summary>
    /// Creates the fresh players for the game.  
    /// Each Player has a role. 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Player> CreateFreshPlayersForGame(List<User> users, Game game, IEnumerable<Room> rooms)
    {
        // Shuffles the roles and assigns the role to each player while creating the player. 
        // I must to this at the very end I think, using a for() loop because I also need to put some dependencies 
        IEnumerable<Roles> allShuffledRoles = _randomProvider.Shuffle(Enum.GetValues<Roles>());
        //
        if (users.Count() != allShuffledRoles.Count())
        {
            throw new ArgumentException(
                "Must have as many users as roles in a game ");
        }

        //TODO; 

        List<Player> players = new List<Player>();

        foreach (var allShuffledRole in allShuffledRoles)
        {
            var createdPlayer = _roleInitializers.First(x => x.Role == allShuffledRole);
            
        }


        // var players = _roleInitializers.Select(x =>
        // {
        //     return new RoleInitializationContext()
        //     {
        //         Game = game,
        //         Rooms = rooms
        //     };
        // });

        return players;
    }
}