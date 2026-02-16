using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.Features.ProductFeature.Queries.GetProductInfoUseCase;

public class GetProductInfosHandler : IRequestHandler<GetProductDtoRequest, ProductDto>
{
    private readonly ErpContext _erpContext;

    public GetProductInfosHandler(ErpContext erpContext)
    {
        _erpContext = erpContext;
    }

    public async ValueTask<ProductDto> Handle(GetProductDtoRequest request, CancellationToken cancellationToken)
    {
        var product = _erpContext.Products.First(x => x.Id == request.ProductId);

        var productItemsInTheCorrectCompany = _erpContext.ProductItems
            .Include(x => x.Warehouse)
                .ThenInclude(x => x.Company)
            .Where(x => x.ProductId == request.ProductId && x.Warehouse.CompanyId == request.CompanyId)
            .ToList();

        return new ProductDto
        {
            ProductId = Guid.NewGuid(),
            ProductName = product.ProductName,
            ProductItemDtos = productItemsInTheCorrectCompany.Select(ProductItemToProductItemDto).ToList(),
        };
    }

    private ProductItemDto ProductItemToProductItemDto(ProductItem productItem)
    {
        ProductItemDto productItemDto = new ProductItemDto()
        {
            Quantity = productItem.Quantity,
            WarehouseName = productItem.Warehouse.WarehouseName,
            WarehouseId = productItem.WarehouseId
        };

        return productItemDto;
    }
}