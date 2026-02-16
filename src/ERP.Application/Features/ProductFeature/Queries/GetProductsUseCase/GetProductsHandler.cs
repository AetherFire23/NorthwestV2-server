using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.ProductFeature.Queries.GetProductsUseCase;

public class GetProductsHandler : IRequestHandler<GetProductsRequest, IEnumerable<Product>>
{
    // TODO : Keep a cache with redis ?
    private readonly ErpContext _erpContext;

    public GetProductsHandler(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<IEnumerable<Product>> Handle(GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        List<Product> products = await _erpContext.Products.ToListAsync();

        return products;
    }
}