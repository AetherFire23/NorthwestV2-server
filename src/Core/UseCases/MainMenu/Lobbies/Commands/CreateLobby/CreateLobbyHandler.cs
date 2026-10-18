using Mediator;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.MainMenu.Lobbies.Commands.JoinLobby;

namespace NorthwestV2.Features.UseCases.MainMenu.Lobbies.Commands.CreateLobby;

public class CreateLobbyHandler : IRequestHandler<CreateLobbyRequest, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly ILobbyRepository _lobbyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateLobbyHandler(IMediator mediator, IUnitOfWork unitOfWork, IUserRepository userRepository,
        ILobbyRepository lobbyRepository)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _lobbyRepository = lobbyRepository;
    }

    public async ValueTask<Guid> Handle(CreateLobbyRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetById(request.UserId);

        Lobby newLobby = Lobby.Create(user);

        _lobbyRepository.Add(newLobby);

        // Make user Join his own Lobby.
        // TODO: Use joinLobby use case 


        await _unitOfWork.SaveChangesAsync();

        await _mediator.Send(new JoinLobbyRequest()
        {
            LobbyId = newLobby.Id,
            UserId = request.UserId
        }, cancellationToken);

        return newLobby.Id;
    }
}