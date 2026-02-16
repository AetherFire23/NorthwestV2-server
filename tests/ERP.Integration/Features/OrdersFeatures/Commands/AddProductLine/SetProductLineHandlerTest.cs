using AetherFire23.ERP.Domain.Entity.Orders;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Features.OrdersFeatures.Commands.AddProductLine;
using Xunit.Abstractions;

namespace ERP.Integration.Features.OrdersFeatures.Commands.AddProductLine;

[TestSubject(typeof(SetProductLineHandler))]
public class SetProductLineHandlerTest : ErpIntegrationTestBase
{
    public SetProductLineHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    // TODO 

    private const int ANY_INITIAL_QUANTITY = 4;
    private const int ANY_MODIFIED_QUANTITY = 4;
    [Fact]
    public async Task GivenProductLine_WhenSet_ThenIsCreated()
    {
        (Guid warehouseId, Guid productId, Guid companyId, Guid orderId) stuff = await CreateCompanyAndWarehouseAndProductsAndSameProductInDifferentWarehouses();

        await base.Mediator.Send(new SetProductLineRequest
        {
            OrderId = stuff.orderId,
            Product = stuff.productId,
            Quantity = 5
        });

        ICollection<OrderProductLine> orders = Context.Orders.Include(x => x.OrderProductLines).Single().OrderProductLines;

        base.Output.WriteLine("WHen productline created, productlines inside of order exists");
        Assert.NotEmpty(orders);
    }

    [Fact]
    public async Task GivenTwoProductLines_WhenAnyQuantitySet_ThenOnlyOneIsSet()
    {
        (Guid warehouseId, Guid productId, Guid companyId, Guid orderId) stuff = await CreateCompanyAndWarehouseAndProductsAndSameProductInDifferentWarehouses();

        await base.Mediator.Send(new SetProductLineRequest
        {
            OrderId = stuff.orderId,
            Product = stuff.productId,
            Quantity = ANY_INITIAL_QUANTITY
        });
        await base.Mediator.Send(new SetProductLineRequest
        {
            OrderId = stuff.orderId,
            Product = stuff.productId,
            Quantity = ANY_MODIFIED_QUANTITY
        });

        var order = Context.Orders
            .Include(x => x.OrderProductLines)
            .Single().OrderProductLines
            .Single();

        base.Output.WriteLine("WHen productline created, productlines inside of order exists");
        Assert.Equal(ANY_MODIFIED_QUANTITY, order.QuantityOrdered);
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