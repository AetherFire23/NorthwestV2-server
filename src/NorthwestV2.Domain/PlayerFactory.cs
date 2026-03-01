using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain;

//TODO: Maybe rename to InitialzePlayersForgameService
public class PlayerFactory
{
    private readonly IRandomProvider _randomProvider;

    public PlayerFactory(IRandomProvider randomProvider)
    {
        _randomProvider = randomProvider;
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
    public IEnumerable<Player> CreateFreshPlayersForGame()
    {
        IEnumerable<Roles> allRoles = _randomProvider.Shuffle(Enum.GetValues<Roles>());

        for (int i = 0; i < GameSettings.RequiredPlayerCountToStartGame; i++)
        {
            // TODO: Create players with their associated role. 
        }

        return [];
    }
}