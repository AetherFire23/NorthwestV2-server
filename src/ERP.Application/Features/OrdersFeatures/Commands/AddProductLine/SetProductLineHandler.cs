using AetherFire23.ERP.Domain.Entity.Orders;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.OrdersFeatures.Commands.AddProductLine;

public class SetProductLineHandler : IRequestHandler<SetProductLineRequest>
{
    private readonly ErpContext _erpContext;

    public SetProductLineHandler(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<Unit> Handle(SetProductLineRequest request, CancellationToken cancellationToken)
    {
        // Getting the relevant entities 
        Order? order = await _erpContext.Orders
            .Include(x => x.OrderProductLines)
                .ThenInclude(x => x.Product)
            .FirstAsync(x => x.Id == request.OrderId, cancellationToken);

        // Order may already have a product line 
        if (order.HasLine(request.Product))
        {
            order.GetLine(request.Product).QuantityOrdered = request.Quantity;
        }
        else
        {
            var product = _erpContext.Products.First(x => x.Id == request.Product);
            order.AddProductLine(product, request.Quantity);
        }

        _erpContext.Update(order);
        // await _erpContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
