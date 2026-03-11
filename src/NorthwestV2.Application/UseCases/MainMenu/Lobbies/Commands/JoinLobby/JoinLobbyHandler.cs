using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Application.Repositories;

namespace NorthwestV2.Application.UseCases.MainMenu.Lobbies.Commands.JoinLobby;

/// <summary>
/// Makes a user join a lobby
/// 1 lobby per user 
/// </summary>
public class JoinLobbyHandler : IRequestHandler<JoinLobbyRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ILobbyRepository _lobbyRepository;

    public JoinLobbyHandler(IUnitOfWork unitOfWork, ILobbyRepository lobbyRepository, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _lobbyRepository = lobbyRepository;
        _userRepository = userRepository;
    }

    public async ValueTask<Guid> Handle(JoinLobbyRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetById(request.UserId);


        Lobby lobby = await _lobbyRepository.GetById(request.LobbyId);

        user.Lobby = lobby;

        await _unitOfWork.SaveChangesAsync();

        return lobby.Id;
    }
}