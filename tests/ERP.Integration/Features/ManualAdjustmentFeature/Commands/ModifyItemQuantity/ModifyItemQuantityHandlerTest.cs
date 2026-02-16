using JetBrains.Annotations;
using NorthwestV2.Application.Features.ManualAdjustmentFeature.Commands.ModifyItemQuantity;
using NorthwestV2.Practical;
using Xunit.Abstractions;

namespace ERP.Integration.Features.ManualAdjustmentFeature.Commands.ModifyItemQuantity;

[TestSubject(typeof(ModifyItemQuantityHandler))]
public class ModifyItemQuantityHandlerTest : ErpIntegrationTestBase
{
    public ModifyItemQuantityHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenProductItemDoesNotExist_WhenProductItemIsSet_ThenProductItemIsAddedWithCorrectQuantity()
    {
        (Guid warehouseId, Guid productId) = await CreateCompanyAndWarehouseAndProducts();

        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 420,
            ProductId = productId,
            WarehouseId = warehouseId
        });

        Assert.Equal(420, base.GetService<ErpContext>().ProductItems.First().Quantity);
    }

    [Fact]
    public async Task GivenProductItemExists_WhenProductItemIsSet_ThenProductItemModifiedWithCorrectQuantity()
    {
        (Guid warehouseId, Guid productId) = await CreateCompanyAndWarehouseAndProducts();
        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 69,
            ProductId = productId,
            WarehouseId = warehouseId
        });

        await base.Mediator.Send(new ModifyItemQuantityRequest
        {
            NewQuantity = 420,
            ProductId = productId,
            WarehouseId = warehouseId
        });

        Assert.NotEmpty(base.Context.ProductItems);
    }

    private async Task<(Guid warehouseId, Guid productId)> CreateCompanyAndWarehouseAndProducts()
    {
        var obj = await base.Mediator.Send(new CreateCompanyRequest
        {
            Password = "BONJOUR",
            CompanyName = "Fredco",
            AdminUserName = "Dieu Seigneur Cieul"
        });

        var warehouseId = await base.Mediator.Send(new CreateWarehouseRequest
        {
            CompanyId = obj.CompanyId,
            WarehouseName = "quebec"
        });

        var productId = await base.Mediator.Send(new CreateProductRequest
        {
            BasePrice = 123,
            ProductName = "La coke de Titin"
        });

        return (warehouseId, productId);
    }
}