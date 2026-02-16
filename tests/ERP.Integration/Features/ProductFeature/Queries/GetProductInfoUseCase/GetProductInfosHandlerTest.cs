
using JetBrains.Annotations;
using NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;
using NorthwestV2.Application.Features.ManualAdjustmentFeature.Commands.ModifyItemQuantity;
using NorthwestV2.Application.Features.ProductFeature.Commands.ProductCreation;
using NorthwestV2.Application.Features.ProductFeature.Queries.GetProductInfoUseCase;
using NorthwestV2.Application.Features.WarehouseFeature.Commands;
using Xunit.Abstractions;

namespace ERP.Integration.Features.ProductFeature.Queries.GetProductInfoUseCase;

[TestSubject(typeof(GetProductInfosHandler))]
public class GetProductInfosHandlerTest : ErpIntegrationTestBase
{
    public GetProductInfosHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    private const int ProductItemsCreated = 3;

    [Fact]
    public async Task GivenProductItemsExist_WhenQueried_ProductItemsDtosAppear()
    {
        var s = await CreateCompanyAndWarehouseAndProductsAndSameProductInDifferentWarehouses();

        var productDto = await base.Mediator.Send(new GetProductDtoRequest
        {
            ProductId = s.productId,
            CompanyId = s.companyId
        });

        Assert.Equal(ProductItemsCreated, productDto.ProductItemDtos.Count);
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

        await base.Mediator.Send(request: new ModifyItemQuantityRequest
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