using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Register;
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
        await RegisterUsersAndStartGame();
        Player anyPlayer = Context.Players.First();


        GetActionsResult response = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = anyPlayer.Id,
        });

        Assert.NotNull(response);
    }

    private async Task RegisterUsersAndStartGame()
    {
        List<Guid> userIds = new List<Guid>();

        for (int i = 0; i < 12; i++)
        {
            Guid userId = await Mediator.Send(new RegisterRequest
            {
                Username = $"User{i}",
                Password = "123"
            });

            userIds.Add(userId);
        }

        await Mediator.Send(new CreateGameRequest
        {
            UserIds = userIds
        });
    }
}