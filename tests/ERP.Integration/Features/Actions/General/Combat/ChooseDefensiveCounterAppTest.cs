using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.General.Combat;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameStart;
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

        var availableStances = getActionsResult.Actions
            .First(x => x.Name == ActionNames.PickDefensiveStance);
        Assert.Equal(2, availableStances.Prompts.First().ValidTargets.Count);
    }

    private async Task<CreateGameSeedData> ArrangeUntilGameCreation()
    {
        List<Guid> ids = new List<Guid>();

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