using NorthwestV2.ApplicationsStuff.EfCoreExtensions;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Repositories;

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