using Mediator;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.Items.Commands;

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
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);
        ItemBase transferredItem = await _itemRepository.Find(request.ItemId);

        player.SwapItemBetweenPlayerOrRoom(transferredItem);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}