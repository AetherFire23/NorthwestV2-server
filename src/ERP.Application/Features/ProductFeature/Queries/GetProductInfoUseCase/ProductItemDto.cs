namespace NorthwestV2.Application.Features.ProductFeature.Queries.GetProductInfoUseCase;

public class ProductItemDto
{
    public required Guid WarehouseId { get; set; }
    public required string WarehouseName { get; set; }
    public required int Quantity { get; set; }
}