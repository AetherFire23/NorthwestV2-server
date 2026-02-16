using Mediator;

namespace NorthwestV2.Application.Features.PurchaseOrderFeature.Commands;

public class CreatePurchaseOrderHandler : IRequestHandler<CreatePurchaseOrderRequest, Guid>
{
    public async ValueTask<Guid> Handle(CreatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        throw new Exception("sd");
    }
}