using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.Extensions.Logging;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.ProductFeature.Commands.ProductCreation;

public class CreateProductHandler : IRequestHandler<CreateProductRequest, Guid>
{
    private readonly ErpContext _erpContext;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(ErpContext erpContext, ILogger<CreateProductHandler> logger)
    {
        _erpContext = erpContext;
        _logger = logger;
    }

    public async ValueTask<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        Product product = new Product
        {
            ProductName = request.ProductName,
            BasePrice = request.BasePrice,
        };

        _erpContext.Products.Add(product);

        await _erpContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Added product {request.ProductName} with price {request.BasePrice}");

        return product.Id;
    }
}