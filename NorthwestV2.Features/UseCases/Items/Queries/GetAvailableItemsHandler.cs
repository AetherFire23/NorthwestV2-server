using Mediator;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.UseCases.Items.Queries;

/// <summary>
/// Returns the Item Dtos of the player and the current room of the player. 
/// </summary>
public class GetAvailableItemsHandler : IRequestHandler<GetAvailableItemsRequest, GetAvailableItemsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;

    public GetAvailableItemsHandler(IUnitOfWork unitOfWork, IPlayerRepository playerRepository)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
    }

    public async ValueTask<GetAvailableItemsResponse> Handle(GetAvailableItemsRequest request,
        CancellationToken cancellationToken)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);

        List<ItemDto> playerItems = player.Inventory.Items.Select(x => x.ToDto()).ToList();
        List<ItemDto> roomItems = player.Room.Inventory.Items.Select(x => x.ToDto()).ToList();

        GetAvailableItemsResponse response = new GetAvailableItemsResponse
        {
            PlayerItems = playerItems,
            RoomItems = roomItems,
        };
        return response;
    }
}