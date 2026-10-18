using Mediator;

namespace NorthwestV2.UseCases.MainMenu.Lobbies.Commands.JoinLobby;

public class JoinLobbyRequest : IRequest<Guid>
{
    public required Guid UserId { get; set; }

    public required Guid LobbyId { get; set; }
}