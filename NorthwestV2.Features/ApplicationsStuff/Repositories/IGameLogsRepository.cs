namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface IGameLogsRepository
{
    public Task GetAllForPlayer(Guid playerId);
}