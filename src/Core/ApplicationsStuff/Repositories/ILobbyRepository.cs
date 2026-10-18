using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.ApplicationsStuff.Repositories;

public interface ILobbyRepository
{
    public void Add(Lobby lobby);

    public Task<Lobby> GetById(Guid id);
}