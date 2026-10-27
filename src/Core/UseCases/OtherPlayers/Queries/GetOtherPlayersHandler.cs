using Mediator;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Shared;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.OtherPlayers.Queries;

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
            Players = playersInSameRoom.Select(MapOtherPlayerDto).ToList()
        };

        await _unitOfWork.SaveChangesAsync();

        return response;
    }

    private static OtherPlayerDto MapOtherPlayerDto(Player player)
    {
        OtherPlayerDto dto = new OtherPlayerDto
        {
            Id = player.Id,
            Role = player.Role,
            HealthIncrement = StatIncrements.From(player.Health),
        };

        return dto;
    }
}