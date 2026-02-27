using Xunit.Abstractions;

namespace NorthwestV2.Integration;

public class UnitTest1 : NorthwestIntegrationTestBase
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
    }
}