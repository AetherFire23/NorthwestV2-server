using AetherFire23.ERP.Domain.Entity;

namespace NorthwestV2.Application.Repositories;

public interface ILobbyRepository
{
    public void Add(Lobby lobby);

    public Task<Lobby> GetById(Guid id);
}