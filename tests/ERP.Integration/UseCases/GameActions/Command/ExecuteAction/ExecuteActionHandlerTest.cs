using JetBrains.Annotations;
using NorthwestV2.Features;
using NorthwestV2.Features.ApplicationsStuff.EfCoreExtensions;
using NorthwestV2.Features.Features;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Authentication.Register;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameStart;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.GameActions.Command.ExecuteAction;

[TestSubject(typeof(ExecuteActionHandler))]
public class ExecuteActionHandlerTest : TestBase2
{
    public ExecuteActionHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenInstantAction_WhenExecuted_ThenHasEffectApplied()
    {
        await RegisterUsersAndStartGame();
        Guid playerId = Context.Players.First().Id;
        var s = await Mediator.Send(new ExecuteActionRequest
        {
            PlayerId = playerId,
            ActionName = ActionNames.InstantHeal
        });

        Player player = await Context.Players.FindById(playerId);
        Assert.True(player.Health == GameSettings.DefaultHealth + 2);
    }

    [Fact]
    public async Task GivenActionWithInvalidTargets_WhenExecuted_ThenThrows()
    {
        await RegisterUsersAndStartGame();
        Guid playerId = Context.Players.First().Id;
        var s = async () => await Mediator.Send(new ExecuteActionRequest
        {
            PlayerId = playerId,
            ActionName = ActionNames.DebugWithTargets,
            ActionTargets =
            [
                [
                    new ActionTarget
                    {
                        Value = "1",
                    }
                ],
                [
                    new ActionTarget
                    {
                        Value = "2",
                    }
                ],
            ]
        });

        await Assert.ThrowsAsync<Exception>(s);
    }

    /// <summary>
    /// 12 users will exist and game would be started 
    /// </summary>
    private async Task RegisterUsersAndStartGame()
    {
        List<Guid> userIds = new();

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