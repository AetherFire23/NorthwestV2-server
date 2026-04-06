using JetBrains.Annotations;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Authentication.Register;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Features.UseCases.GameStart;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.GameActions.Queries.GetActions;

[TestSubject(typeof(GetActionsHandler))]
public class GetActionsHandlerTest : TestBase2
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

        Assert.NotEmpty(response.Actions);
    }
    
    [Fact]
    public async Task GivenDebugActionWithTargets_WhenGetGameActions_ThenTargetsAreVisible()
    {
        await RegisterUsersAndStartGame();
        Player anyPlayer = Context.Players.First();

        GetActionsResult response = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = anyPlayer.Id,
        });

        Assert.NotEmpty(response.Actions);
    }

    /// <summary>
    /// 12 users will exist and game would be started 
    /// </summary>
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