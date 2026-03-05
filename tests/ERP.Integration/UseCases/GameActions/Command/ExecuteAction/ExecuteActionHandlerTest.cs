using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameStart;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.GameActions.Command.ExecuteAction;

[TestSubject(typeof(ExecuteActionHandler))]
public class ExecuteActionHandlerTest : NorthwestIntegrationTestBase
{
    public ExecuteActionHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenInstantAction_WhenExecuted_ThenHasEffectApplied()
    {
        await RegisterUsersAndStartGame();

        await Mediator.Send(new ExecuteActionRequest()
        {
            PlayerId = Context.Players.First().Id,
            ActionName = "Self heal instant"
        });
        
        
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