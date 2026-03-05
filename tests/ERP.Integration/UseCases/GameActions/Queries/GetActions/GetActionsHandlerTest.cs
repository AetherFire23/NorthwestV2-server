using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameStart;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions;

[TestSubject(typeof(GetActionsHandler))]
public class GetActionsHandlerTest : NorthwestIntegrationTestBase
{
    public GetActionsHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    // TODO: This test requires that players exist, etc. 
    [Fact]
    public async Task GivenAnyPlayerInGame_WhenGetGameActionsCalled_ThenIsSomeDebugActionRetrievable()
    {
        Player anyPlayer = Context.Players.First();


        GetActionsResult response = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = anyPlayer.Id,
        });

        Assert.NotNull(response);
    }

    private void RegisterUsersAndStartGame()
    {
        // await Mediator.Send(new CreateGameHandler())
        // {
        //     
        // }
    }
}