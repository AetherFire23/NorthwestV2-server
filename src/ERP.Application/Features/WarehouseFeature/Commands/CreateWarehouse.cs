using AetherFire23.ERP.Domain.Entity;
using Mediator;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.WarehouseFeature.Commands;

/// <summary>
/// An admin / buyer / owner can create a new warehouse. 
/// </summary>
public class CreateWarehouse : IRequestHandler<CreateWarehouseRequest, Guid>
{
    private readonly ErpContext _erpContext;

    public CreateWarehouse(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<Guid> Handle(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouse = new Warehouse
        {
            WarehouseName = request.WarehouseName,
            CompanyId = request.CompanyId,
        };

        _erpContext.Warehouses.Add(warehouse);

        await _erpContext.SaveChangesAsync(cancellationToken);

        return warehouse.Id;
    }
}