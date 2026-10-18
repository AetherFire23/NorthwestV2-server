using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.ApplicationsStuff.Repositories;

public interface ILobbyRepository
{
    public void Add(Lobby lobby);

    public Task<Lobby> GetById(Guid id);
}