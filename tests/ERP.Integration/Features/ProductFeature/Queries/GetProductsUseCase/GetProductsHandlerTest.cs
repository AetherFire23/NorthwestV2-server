using JetBrains.Annotations;
using NorthwestV2.Application.Features.ProductFeature.Queries.GetProductsUseCase;
using Xunit.Abstractions;

namespace ERP.Integration.Features.ProductFeature.Queries.GetProductsUseCase;

[TestSubject(typeof(GetProductsHandler))]
public class GetProductsHandlerTest : ErpIntegrationTestBase
{
    public GetProductsHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenProducts_WhenFetched_ExistInCollection()
    {
        var seed = await CreateCompanyAndWarehouseAndProductsAndSameProductInDifferentWarehouses();

        var products = await base.Mediator.Send(new GetProductsRequest() { });

        Assert.NotEmpty(products);
    }

    private async Task<(Guid warehouseId, Guid productId, Guid companyId)>
        CreateCompanyAndWarehouseAndProductsAndSameProductInDifferentWarehouses()
    {
        var obj = await base.Mediator.Send(new CreateCompanyRequest
        {
            Password = "BONJOUR",
            CompanyName = "Fredco",
            AdminUserName = "Dieu Seigneur Cieul"
        });

        // Same product


        var productId = await base.Mediator.Send(new CreateProductRequest
        {
            BasePrice = 123,
            ProductName = "La coke de Titin"
        });

        // Different warehouses 
        var warehouseId = await base.Mediator.Send(new CreateWarehouseRequest
        {
            CompanyId = obj.CompanyId,
            WarehouseName = "quebec"
        });

        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 55,
            ProductId = productId,
            WarehouseId = warehouseId,
        });

        var warehouseId2 = await base.Mediator.Send(new CreateWarehouseRequest
        {
            CompanyId = obj.CompanyId,
            WarehouseName = "saint-jean"
        });

        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 420,
            ProductId = productId,
            WarehouseId = warehouseId2,
        });

        var warehouseId3 = await base.Mediator.Send(new CreateWarehouseRequest
        {
            CompanyId = obj.CompanyId,
            WarehouseName = "montreal"
        });

        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 69,
            ProductId = productId,
            WarehouseId = warehouseId3,
        });

        var differentProductInSameWarehouse = await base.Mediator.Send(new CreateProductRequest
        {
            BasePrice = 123,
            ProductName = "Le diffrent produit "
        });

        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 13,
            ProductId = differentProductInSameWarehouse,
            WarehouseId = warehouseId,
        });

        return (warehouseId, productId, obj.CompanyId);
    }
}