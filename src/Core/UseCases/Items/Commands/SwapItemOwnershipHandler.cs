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
        Player player = await _playerRepository.GetPlayerAndRoomAndInventoryAndGame(request.PlayerId);
        ItemBase transferredItem = await _itemRepository.Find(request.ItemId);

        player.SwapItemBetweenPlayerOrRoom(transferredItem);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}