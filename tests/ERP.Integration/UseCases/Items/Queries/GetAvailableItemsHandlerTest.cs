using JetBrains.Annotations;
using NorthwestV2.Features.UseCases.Items.Queries;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Items.Queries;

[TestSubject(typeof(GetAvailableItemsHandler))]
public class GetAvailableItemsHandlerTest : TestBase2, IAsyncLifetime
{
    public GetAvailableItemsHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    public async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
    }

    [Fact]
    public async Task GivenPlayerInRoom_WhenGetAvailableItems_ThenHisItemsArePresent()
    {
        
        // TODO: Make player dto ! 
        // Assert.True(playerDto.Items)
    }

    [Fact]
    public async Task GivenPlayerInRoom_WithRoomWithItems_ThenRoomItemsArePresent()
    {
    }
}