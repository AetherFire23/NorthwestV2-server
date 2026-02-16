using JetBrains.Annotations;
using NorthwestV2.Application.Features.WarehouseFeature.Commands;
using Xunit.Abstractions;

namespace ERP.Integration.Features.WarehouseFeature.Commands;

[TestSubject(typeof(CreateWarehouse))]
public class CreateWarehouseTest : ErpIntegrationTestBase
{
    public CreateWarehouseTest(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public async Task GivenAManager_CreatesAWarehouse_WarehouseExists()
    {
        await Mediator.Send(new CreateCompanyRequest
        {
            CompanyName = "FredCo",
            AdminUserName = "admin",
            Password = "Bonjour"
        });

        await Mediator.Send(new CreateProductRequest
        {
            BasePrice = 14,
            ProductName = "TN760 REU",
        });

        // await Mediator.Send(new CreateWarehouseRequest
        // {
        //     CompanyId =
        //         WarehouseName = "Quebec",
        // });
    }
}