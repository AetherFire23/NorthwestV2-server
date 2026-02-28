using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.Lobbies.Commands.JoinLobby;

/// <summary>
/// Makes a user join a lobby 
/// </summary>
public class JoinLobbyHandler : IRequestHandler<JoinLobbyRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;

    public JoinLobbyHandler(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public async ValueTask<Guid> Handle(JoinLobbyRequest request, CancellationToken cancellationToken)
    {
        User user = await _northwestContext.Users.FindAsync(request.UserId) ?? throw new Exception("User not found");

        Lobby lobby = await _northwestContext.Lobbies.FindAsync(request.LobbyId) ??
                      throw new Exception("Lobby not found");

        user.Lobby = lobby;

        await _northwestContext.SaveChangesAsync(cancellationToken);

        return Guid.Empty;
    }
}