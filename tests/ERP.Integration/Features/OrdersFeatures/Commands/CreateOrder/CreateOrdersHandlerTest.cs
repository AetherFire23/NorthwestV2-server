using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;
using NorthwestV2.Application.Features.ManualAdjustmentFeature.Commands.ModifyItemQuantity;
using NorthwestV2.Application.Features.OrdersFeatures.Commands.AddProductLine;
using NorthwestV2.Application.Features.OrdersFeatures.Commands.CreateOrder;
using NorthwestV2.Application.Features.ProductFeature.Commands.ProductCreation;
using NorthwestV2.Application.Features.WarehouseFeature.Commands;
using Xunit.Abstractions;

namespace ERP.Integration.Features.OrdersFeatures.Commands.CreateOrder;

[TestSubject(typeof(CreateOrdersHandler))]
public class CreateOrdersHandlerTest : ErpIntegrationTestBase
{
    public CreateOrdersHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenOrder_WhenCreated_ThenExistsInContext()
    {
        var tup = await CreateCompanyAndWarehouseAndProductsAndSameProductInDifferentWarehouses();

        await Mediator.Send(new SetProductLineRequest()
        {
            OrderId = tup.orderId,
            Product = tup.productId,
            Quantity = 12,
        });

        var order = Context.Orders
            .Include(x => x.OrderProductLines)
            .First(x => x.Id == tup.orderId);

        Assert.NotEmpty(order.OrderProductLines);
    }

    private async Task<(Guid warehouseId, Guid productId, Guid companyId, Guid orderId)>
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

        Guid orderId = await base.Mediator.Send(new CreateOrderRequest());

        return (warehouseId, productId, obj.CompanyId, orderId);
    }
}