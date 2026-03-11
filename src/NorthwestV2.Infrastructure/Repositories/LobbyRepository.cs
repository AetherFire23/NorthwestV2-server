using AetherFire23.ERP.Domain.Entity;
using NorthwestV2.Application.EfCoreExtensions;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Practical;

namespace NorthwestV2.Infrastructure.Repositories;

public class LobbyRepository : ILobbyRepository
{
    private readonly NorthwestContext _northwestContext;

    public LobbyRepository(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public void Add(Lobby lobby)
    {
        _northwestContext.Lobbies.Add(lobby);
    }

    public async Task<Lobby> GetById(Guid id)
    {
        Lobby lobby = await _northwestContext.Lobbies.FindById(id);

        return lobby;
    }
}