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

    public async Task<IReadOnlyList<GameLog>> GetAllForPlayer(Guid playerId)
    {
        Player player = await _context
            .Players
            .Include(x => x.GameLogs)
            .FirstAsync(x => x.Id == playerId);

        List<GameLog> logs = player.GameLogs.ToList();
        return logs;
    }
}