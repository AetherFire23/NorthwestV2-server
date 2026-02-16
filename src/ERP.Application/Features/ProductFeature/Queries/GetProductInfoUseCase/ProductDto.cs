namespace NorthwestV2.Application.Features.ProductFeature.Queries.GetProductInfoUseCase;

/// <summary>
/// Gets all the information about the different quantities of the product in the different warehouses.   
/// </summary>
public class ProductDto
{
    public required Guid ProductId { get; set; }
    public required string ProductName { get; set; }

    public required List<ProductItemDto> ProductItemDtos { get; set; }
}