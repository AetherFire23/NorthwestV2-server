using JetBrains.Annotations;
using NorthwestV2.Features.Features;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.General.Combat.ChooseDefensiveCounter;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Authentication.Register;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Features.UseCases.GameStart;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

[TestSubject(typeof(ChooseDefensiveCounterApp))]
public class ChooseDefensiveCounterAppTest : TestBase2
{
    public ChooseDefensiveCounterAppTest(ITestOutputHelper output) : base(output)
    {
    }

    // TODO: All enum values are correctly Converted into targets for roles. 
    [Fact]
    public async Task GivenDefensiveStances_WhenGetAvailability_ThenAreAllRolesConvertedToTargets()
    {
        CreateGameSeedData data = await ArrangeUntilGameCreation();

        GetActionsResult getActionsResult = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = data.PlayerIds.First(),
        });

        // Only 2 stances, won't do a complex algorithm to test that. 

        ActionDto availableStances = getActionsResult.Actions
            .First(x => x.Name == ActionNames.ChooseDefensiveStance);
        Assert.Equal(2, availableStances.Prompts.First().ValidTargets.Count);
    }

    /*
     * Execution tests
     */
    [Fact]
    public async Task GivenAnyDefensiveStance_WhenChoosingStanceAction_ThenPickedDefensiveStanceSelected()
    {
        CreateGameSeedData data = await ArrangeUntilGameCreation();
        Guid playerId = data.PlayerIds.First();
        GetActionsResult getActionsResult = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = playerId,
        });

        ActionDto availableStances = getActionsResult.Actions
            .First(x => x.Name == ActionNames.ChooseDefensiveStance);
        ActionTarget stance = availableStances.Prompts.First().ValidTargets
            .First(x => x.Value == DefensiveCounters.Override.ToString());
        await Mediator.Send(new ExecuteActionRequest()
        {
            ActionName = ActionNames.ChooseDefensiveStance,
            PlayerId = playerId,
            ActionTargets = [[stance]]
        });

        // Only 2 stances, won't do a complex algorithm to test that. 
        Player player = await PlayerRepository.GetPlayer(playerId);
        Assert.Equal(DefensiveCounters.Override, player.DefensiveCounter);
    }


    private async Task<CreateGameSeedData> ArrangeUntilGameCreation()
    {
        List<Guid> ids = [];

        for (int i = 0; i < GameSettings.RequiredPlayerCountToStartGame; i++)
        {
            Guid userId = await Mediator.Send(new RegisterRequest()
            {
                Username = $"User{i}",
                Password = "123"
            });

            ids.Add(userId);
        }

        Guid gameId = await Mediator.Send(new CreateGameRequest()
        {
            UserIds = ids
        });


        List<Guid> playerIds = Context.Players
            .Where(x => x.GameId == gameId)
            .Select(x => x.Id)
            .ToList();

        return new CreateGameSeedData()
        {
            UserIds = ids,
            GameId = gameId,
            PlayerIds = playerIds
        };
    }

    private class CreateGameSeedData
    {
        public required Guid GameId { get; set; }
        public required IEnumerable<Guid> UserIds { get; set; }
        public required IEnumerable<Guid> PlayerIds { get; set; }
    }
}