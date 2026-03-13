using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Practical;

namespace NorthwestV2.Infrastructure.Repositories;

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