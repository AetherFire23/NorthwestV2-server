using Mediator;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Shared;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.MyPlayer.Queries;

public class GetMyPlayerHandler : IRequestHandler<GetMyPlayerRequest, GetMyPlayerResponse>
{
    private readonly IPlayerRepository _playerRepository;

    public GetMyPlayerHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async ValueTask<GetMyPlayerResponse> Handle(GetMyPlayerRequest request,
        CancellationToken cancellationToken)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        GetMyPlayerResponse response = new GetMyPlayerResponse
        {
            Id = player.Id,
            Role = player.Role,
            LifeStatus = player.LifeStatus,
            IsAcolyte = player.IsAcolyte,
            HealthIncrement = StatIncrements.From(player.Health),
            HopeIncrement = StatIncrements.From(player.Hope),
            IsVigilant = player.IsVigilant,
        };

        return response;
    }
}
