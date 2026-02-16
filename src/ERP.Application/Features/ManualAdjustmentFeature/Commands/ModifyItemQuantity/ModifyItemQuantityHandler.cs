using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.Extensions.Logging;
using NorthwestV2.Practical;
using NorthwestV2.Practical.Repositories;

namespace NorthwestV2.Application.Features.ManualAdjustmentFeature.Commands.ModifyItemQuantity;

public class ModifyItemQuantityHandler : IRequestHandler<ModifyItemQuantityRequest>
{
    private readonly ErpContext _erpContext;
    private readonly WarehouseRepository _warehouseRepository;
    private readonly ILogger<ModifyItemQuantityHandler> _logger;

    public ModifyItemQuantityHandler(ErpContext erpContext, WarehouseRepository warehouseRepository,
        ILogger<ModifyItemQuantityHandler> logger)
    {
        _erpContext = erpContext;
        _warehouseRepository = warehouseRepository;
        _logger = logger;
    }

    public async ValueTask<Unit> Handle(ModifyItemQuantityRequest request, CancellationToken cancellationToken)
    {
        ProductItem? productItem =
            await _warehouseRepository.GetProductItemInWarehouse(request.WarehouseId, request.ProductId);

        if (productItem is null)
        {
            _logger.LogInformation("Product item not found, creating one...");
            Product product = _erpContext.Products.First(p => p.Id == request.ProductId);

            Warehouse warehouse = await _erpContext.Warehouses.FindAsync(request.WarehouseId) ??
                                  throw new Exception("Not found");

            var newProductItem = warehouse.AddProductItem(product, request.NewQuantity);

            _erpContext.Add(newProductItem);

        }
        else
        {
            _logger.LogInformation("Product item found. setting new quantity.");
            productItem.Quantity = request.NewQuantity;
        }

        await _erpContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}