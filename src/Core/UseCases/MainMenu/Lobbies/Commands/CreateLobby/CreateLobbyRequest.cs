using Mediator;

namespace NorthwestV2.Features.UseCases.MainMenu.Lobbies.Commands.CreateLobby;

/// <summary>
///Returns a lobby Id. 
/// </summary>
public class CreateLobbyRequest : IRequest<Guid>
{
    public required Guid UserId { get; set; }
}