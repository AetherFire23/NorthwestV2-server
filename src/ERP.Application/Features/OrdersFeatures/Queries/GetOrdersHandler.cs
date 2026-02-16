using AetherFire23.ERP.Domain.Entity.Orders;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.OrdersFeatures.Queries;

public class GetOrdersHandler : IRequestHandler<GetOrdersRequest, List<Order>>
{
    private readonly ErpContext _erpContext;

    public GetOrdersHandler(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<List<Order>> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        List<Order> orders = await _erpContext.Orders
            .Include(x => x.OrderProductLines)
            .ToListAsync();

        return orders;
    }
}

// OrderProductLine -> 