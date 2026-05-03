using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface IGameLogsRepository
{
    public Task<IReadOnlyList<GameLog>> GetAllForPlayer(Guid playerId);
}