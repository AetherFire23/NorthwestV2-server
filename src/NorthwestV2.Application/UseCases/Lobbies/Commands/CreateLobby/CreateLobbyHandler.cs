using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Application.UseCases.Lobbies.Commands.JoinLobby;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.Lobbies.Commands.CreateLobby;

public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;

    private readonly IMediator _mediator;

    public CreateLobbyHandler(NorthwestContext northwestContext, IMediator mediator)
    {
        _northwestContext = northwestContext;
        _mediator = mediator;
    }

    public async ValueTask<Guid> Handle(CreateLobbyRequest request, CancellationToken cancellationToken)
    {
        User user = await _northwestContext.Users.FindAsync(request.UserId, cancellationToken);

        Lobby newLobby = Lobby.Create(user);

        _northwestContext.Lobbies.Add(newLobby);

        // Make user Join his own Lobby.
        // TODO: Use joinLobby use case 


        await _northwestContext.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new JoinLobbyRequest()
        {
            LobbyId = newLobby.Id,
            UserId = request.UserId
        }, cancellationToken);

        return newLobby.Id;
    }
}