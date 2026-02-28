using Mediator;

namespace NorthwestV2.Application.UseCases.Lobbies.Commands;

public class CreateLobbyRequest : IRequest<Guid>
{
    public required Guid UserId { get; set; }
}