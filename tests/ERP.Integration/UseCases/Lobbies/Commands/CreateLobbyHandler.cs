using Mediator;

namespace NorthwestV2.Integration.UseCases.Lobbies.Commands;

public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest>
{
    public ValueTask<Unit> Handle(CreateLobbyRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}