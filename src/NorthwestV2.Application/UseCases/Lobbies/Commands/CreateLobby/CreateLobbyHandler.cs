using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.Lobbies.Commands;

public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;

    public CreateLobbyHandler(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public async ValueTask<Guid> Handle(CreateLobbyRequest request, CancellationToken cancellationToken)
    {
        User user = await _northwestContext.Users.FindAsync(request.UserId) ??
                    throw new Exception("Userid cannot be null");

        Lobby newLobby = new Lobby();

        _northwestContext.Lobbies.Add(newLobby);

        await _northwestContext.SaveChangesAsync(cancellationToken);

        return newLobby.Id;
    }
}