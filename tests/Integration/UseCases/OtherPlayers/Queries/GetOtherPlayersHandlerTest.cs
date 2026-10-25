using JetBrains.Annotations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Integration.Helpers;
using NorthwestV2.UseCases.OtherPlayers.Queries;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.OtherPlayers.Queries;

[TestSubject(typeof(GetOtherPlayersHandler))]
public class GetOtherPlayersHandlerTest : NorthwestTestBase
{
    private GameDataSeed _gameDataSeed;

    public GetOtherPlayersHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
    }

    [Fact]
    public async Task GivenTwoPlayersInSameRoom_WhenGettingOthers_ThenOtherPlayerIsDescribedByRole()
    {
        Guid viewerId = _gameDataSeed.PlayerIds[0];
        Guid otherId = _gameDataSeed.PlayerIds[1];
        await this.TeleportPlayerTo(viewerId, RoomEnum.MainDeck);
        await this.TeleportPlayerTo(otherId, RoomEnum.MainDeck);
        Player other = await Context.Players.FindAsync(otherId);

        GetOtherPlayersResponse response = await Mediator.Send(new GetOtherPlayersRequest { PlayerId = viewerId });

        Assert.Contains(response.Players, x => x.Id == otherId && x.Role == other.Role);
    }
}
