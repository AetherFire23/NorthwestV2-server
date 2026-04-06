using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using JetBrains.Annotations;
using NorthwestV2.Application.Features.Actions.General.Movement;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameStart;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Movement;

[TestSubject(typeof(ChangeRoomApp))]
public class ChangeRoomAppTest : TestBase2
{
    public ChangeRoomAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Given_When_Then()
    {
        CreateGameSeedData gameData = await ArrangeUntilGameCreation();
        Guid playerId = gameData.PlayerIds.First();
        var roms = await Mediator.Send(new GetActionsRequest()
        {
            PlayerId = playerId,
        });
        // await Mediator.Send(new ExecuteActionRequest()
        // {
        //     ActionName = ActionNames.ChangeRoom,
        //     PlayerId = playerId
        // });
        
        Assert.NotEmpty(roms.Actions.First(x=> x.Name == ActionNames.ChangeRoom).Prompts.First().ValidTargets);
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