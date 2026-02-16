using AetherFire23.ERP.Domain.Entity;
using Mediator;

namespace NorthwestV2.Application.Features.ProductFeature.Queries.GetProductsUseCase;

public class GetProductsRequest : IRequest<IEnumerable<Product>>
{
    // TODO: Choose strategy of preloading /here ??
}