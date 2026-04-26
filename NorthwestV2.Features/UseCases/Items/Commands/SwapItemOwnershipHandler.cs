using Mediator;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.UseCases.Items.Commands;

public class SwapItemOwnershipHandler : IRequestHandler<SwapItemOwnershipRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly IItemrepository _itemRepository;

    public SwapItemOwnershipHandler(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
        IItemrepository itemRepository)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _itemRepository = itemRepository;
    }

    public async ValueTask<Unit> Handle(SwapItemOwnershipRequest request,
        CancellationToken cancellationToken)
    {
        // Get the player.
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);
        
        //  Ensure that the item is either contained in the player's room OR the current room.
        ItemBase transferredItem = await _itemRepository.Find(request.ItemId);

        player.SwapItemBetweenPlayerOrRoom(transferredItem);

        // Take the inventory, and take ownership of the item.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}