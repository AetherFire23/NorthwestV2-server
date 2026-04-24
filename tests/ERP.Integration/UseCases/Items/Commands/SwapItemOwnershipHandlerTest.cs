using JetBrains.Annotations;
using NorthwestV2.Features.UseCases.Items.Commands;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Items.Commands;

[TestSubject(typeof(SwapItemOwnershipHandler))]
public class SwapItemOwnershipHandlerTest : TestBase2
{
    public SwapItemOwnershipHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenItemInRoom_WhenPlayerTransfers_Then()
    {
        Assert.Fail();
    }
}