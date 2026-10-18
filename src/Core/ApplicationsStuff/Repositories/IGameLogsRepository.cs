using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.ApplicationsStuff.Repositories;

public interface IGameLogsRepository
{
    public Task<IReadOnlyList<GameLog>> GetAllForPlayer(Guid playerId);
}