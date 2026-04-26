using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Infrastructure.Repositories;

public class GameLogsRepository : IGameLogsRepository
{
    private readonly NorthwestContext _context;

    public GameLogsRepository(NorthwestContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<GameLog>> GetAllForPlayer(Guid playerId)
    {
        return await _context.PlayerGameLogs
            .Where(x => x.PlayerId == playerId)
            .Select(x => x.GameLog)
            .ToListAsync();
    }
}