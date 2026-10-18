using JetBrains.Annotations;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Cooking.CookRation;
using NorthwestV2.Features.Actions.Cooking.Items;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.Cooking;

[TestSubject(typeof(CookRationActionApp))]
public class CookRationActionAppTest : NorthwestTestBase
{
    public CookRationActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenCan_WhenExecuting_ThenCanIsConsumed()
    {
        Guid playerId = await SetupPlayerWithCans(1);

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CookRation,
            PlayerId = playerId,
        });
        Player player = await GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);

        Assert.False(player.Inventory.Has<Can>());
    }

    [Fact]
    public async Task GivenCan_WhenExecuting_ThenRationIsProduced()
    {
        Guid playerId = await SetupPlayerWithCans(1);

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CookRation,
            PlayerId = playerId,
        });
        Player player = await GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);

        Assert.True(player.Inventory.Has<Ration>());
    }

    [Fact]
    public async Task GivenNoCan_WhenGettingActions_ThenRationActionIsListedButCannotExecute()
    {
        Guid playerId = await SetupPlayerWithCans(0);

        GetActionsResult result = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });

        Assert.True(result.HasAction(ActionNames.CookRation));
    }

    [Fact]
    public async Task GivenNoCan_WhenExecuting_ThenThrows()
    {
        Guid playerId = await SetupPlayerWithCans(0);

        await Assert.ThrowsAnyAsync<Exception>(async () => await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CookRation,
            PlayerId = playerId,
        }));
    }

    private async Task<Guid> SetupPlayerWithCans(int canCount)
    {
        GameDataSeed seed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid playerId = seed.PlayerIds.First();

        Player player = await GetServiceFromScope<IPlayerRepository>().GetPlayerAndRoomAndInventoryAndGame(playerId);

        for (int i = 0; i < canCount; i++)
        {
            player.Inventory.Items.Add(new Can());
        }

        await Context.SaveChangesAsync();
        return playerId;
    }
}
