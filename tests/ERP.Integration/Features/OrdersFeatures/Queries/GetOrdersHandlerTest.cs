using JetBrains.Annotations;
using NorthwestV2.Application.Features.OrdersFeatures.Queries;
using Xunit.Abstractions;

namespace ERP.Integration.Features.OrdersFeatures.Queries;

[TestSubject(typeof(GetOrdersHandler))]
public class GetOrdersHandlerTest : ErpIntegrationTestBase
{
    public GetOrdersHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Bloop()
    {

    }
}