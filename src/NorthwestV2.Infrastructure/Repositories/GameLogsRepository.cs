using NorthwestV2.Features.ApplicationsStuff.Repositories;

namespace NorthwestV2.Infrastructure.Repositories;

public class GameLogsRepository : IGameLogsRepository
{
    private readonly NorthwestContext _context;

    public GameLogsRepository(NorthwestContext context)
    {
        _context = context;
    }

    public async Task GetAllForPlayer(Guid playerId)
    {
        // Player player = _context.Logs
        
    }
}