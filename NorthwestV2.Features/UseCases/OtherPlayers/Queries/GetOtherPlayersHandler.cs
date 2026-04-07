using Mediator;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.UseCases.OtherPlayers.Queries;

public class GetOtherPlayersHandler : IRequestHandler<GetOtherPlayersRequest, GetOtherPlayersResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;

    public GetOtherPlayersHandler(IUnitOfWork unitOfWork, IPlayerRepository playerRepository)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
    }

    public async ValueTask<GetOtherPlayersResponse> Handle(GetOtherPlayersRequest request,
        CancellationToken cancellationToken)
    {
        List<Player> playersInSameRoom = await _playerRepository.GetPlayersInSameRoom(request.PlayerId);


        GetOtherPlayersResponse response = new GetOtherPlayersResponse()
        {
            PlayerNames = playersInSameRoom.Select(x=> x.Role.ToString()).ToList()
        };

        await _unitOfWork.SaveChangesAsync();

        return response;
    }
}