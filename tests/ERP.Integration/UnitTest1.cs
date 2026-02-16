using Xunit.Abstractions;

namespace ERP.Integration;

public class UnitTest1 : ErpIntegrationTestBase
{
    /// <summary>
    /// Passing the testoutput provider in the output.
    /// </summary>
    /// <param name="output"></param>
    public UnitTest1(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenAManager_SetsProductQuantity_QuantityIsSet()
    {
        await Mediator.Send(new CreateCompanyRequest
        {
            CompanyName = "FredCo",
            AdminUserName = "admin",
            Password = "BONJOUR"
        });

        await Mediator.Send(new CreateProductRequest
        {
            BasePrice = 14,
            ProductName = "TN760 REU",
        });
    }
}