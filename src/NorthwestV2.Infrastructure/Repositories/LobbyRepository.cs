using NorthwestV2.Features.EfCoreExtensions;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;

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