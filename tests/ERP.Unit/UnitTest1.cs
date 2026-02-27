using NorthwestV2.Integration;
using Xunit.Abstractions;

namespace ERP.Testing.Domain;

public class UnitTest1 : NorthwestIntegrationTestBase
{
    public UnitTest1(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Test1()
    {

    }
}