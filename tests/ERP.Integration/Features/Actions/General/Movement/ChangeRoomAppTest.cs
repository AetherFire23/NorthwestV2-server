using JetBrains.Annotations;
using NorthwestV2.Features.Features;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.General.Movement;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Authentication.Register;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Features.UseCases.GameStart;
using NorthwestV2.Integration.Helpers;
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
    public async Task GivenPlayerInAnyRoom_WhenGettingChangeRoomAction_ThenChangeRoomActionIsAvailable()
    {
        CreateGameSeedData gameData = await ArrangeUntilGameCreation();
        Guid playerId = gameData.PlayerIds.First();

        GetActionsResult roms = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });

        ActionDto action = roms.Actions.First(x => x.Name == ActionNames.ChangeRoom);
        bool anyOtheRoomIsAvailableToMoveTo = action.Prompts.First().ValidTargets.Any();
        Assert.True(anyOtheRoomIsAvailableToMoveTo);
    }

    /*
     * The only other room connected to crow's nest is Main Deck, whhichi makes it the best ideal candiate to test for
     * room changes. 
     */
    [Fact]
    public async Task GivenPlayerInAnyRoom_WhenChangingRoom_ThenPlayerIsInDifferentRoom()
    {
        CreateGameSeedData gameData = await ArrangeUntilGameCreation();
        Guid playerId = gameData.PlayerIds.First();
        await this.TeleportPlayerTo(playerId, RoomEnum.CrowsNest);
        GetActionsResult roms = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId,
        });
        ActionDto action = roms.Actions.First(x => x.Name == ActionNames.ChangeRoom);
        await Mediator.Send(new ExecuteActionRequest()
        {
            ActionName = ActionNames.ChangeRoom,
            PlayerId = playerId,
            ActionTargets = [[action.Prompts.First().ValidTargets.First()]]
        });

        Player player = await PlayerRepository.GetPlayer(playerId);

        Assert.Equal(RoomEnum.MainDeck, player.Room.RoomEnum);
    }

    // TODO: More tests for when I will add requirements. 

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