using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.GameStart.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameLogs.Queries;
using NorthwestV2.UseCases.MyPlayer.Queries;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.MyPlayer.Queries;

[TestSubject(typeof(GetMyPlayerHandler))]
public class GetMyPlayerHandlerTest : NorthwestTestBase
{
    private const string LoyalMessage = "You are loyal.";
    private const string FellowAcolytePrefix = "Your fellow acolyte:";

    private GameDataSeed _gameDataSeed;

    public GetMyPlayerHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
    }

    [Fact]
    public async Task GivenNewGame_WhenEachPlayerAsksForThemselves_ThenTwoAcolytesAreReported()
    {
        List<GetMyPlayerResponse> responses = [];
        foreach (Guid playerId in _gameDataSeed.PlayerIds)
        {
            responses.Add(await Mediator.Send(new GetMyPlayerRequest { PlayerId = playerId }));
        }

        Assert.Equal(AllegianceConstants.AcolyteCount, responses.Count(x => x.IsAcolyte));
    }

    [Fact]
    public async Task GivenNewGame_WhenChaplainAsksForThemselves_ThenChaplainIsNotAcolyte()
    {
        Player chaplain = await Context.Players.SingleAsync(x => x.GameId == _gameDataSeed.GameId
                                                                 && x.Role == Roles.Chaplain);

        GetMyPlayerResponse response = await Mediator.Send(new GetMyPlayerRequest { PlayerId = chaplain.Id });

        Assert.False(response.IsAcolyte);
    }

    [Fact]
    public async Task GivenNewGame_WhenAcolyteReadsLogs_ThenLogNamesTheFellowAcolyte()
    {
        Player acolyte = await Context.Players.FirstAsync(x => x.GameId == _gameDataSeed.GameId && x.IsAcolyte);

        GetGameLogsResponse logs = await Mediator.Send(new GetGameLogsRequest { PlayerId = acolyte.Id });

        Assert.Contains(FellowAcolytePrefix, logs.Logs.Single().Message);
    }

    [Fact]
    public async Task GivenNewGame_WhenLoyalReadsLogs_ThenLogSaysLoyal()
    {
        Player loyal = await Context.Players.FirstAsync(x => x.GameId == _gameDataSeed.GameId && !x.IsAcolyte);

        GetGameLogsResponse logs = await Mediator.Send(new GetGameLogsRequest { PlayerId = loyal.Id });

        Assert.Equal(LoyalMessage, logs.Logs.Single().Message);
    }
}
