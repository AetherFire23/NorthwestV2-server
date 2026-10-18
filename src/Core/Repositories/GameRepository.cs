using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Repositories;

public class GameRepository : IGameRepository
{
    private readonly NorthwestContext _northwestContext;

    public GameRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public void Add(Game game)
    {
        _northwestContext.Games.Add(game);
    }
}