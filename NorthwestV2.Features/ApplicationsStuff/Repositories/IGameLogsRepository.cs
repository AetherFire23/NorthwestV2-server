using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface IGameLogsRepository
{
    public Task<IReadOnlyCollection<GameLog>> GetAllForPlayer(Guid playerId);
}