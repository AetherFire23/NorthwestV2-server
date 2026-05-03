using JetBrains.Annotations;
using NorthwestV2.Features.ApplicationsStuff.EfCoreExtensions;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameLogs.Queries;
using NorthwestV2.Integration.Scratches;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.GameLogs.Queries;

[TestSubject(typeof(GetGameLogsHandler))]
public class GetGameLogsHandlerTest : TestBase2
{
    private GameDataSeed _gameDataSeed;
    private Player ANY_PLAYER;
    private Player ANY_OTHER_PLAYER;
    private Player NOT_RELATED_PLAYER;

    public GetGameLogsHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        ANY_PLAYER = await Context.Players.FindById(_gameDataSeed.PlayerIds[0]);
        ANY_OTHER_PLAYER = await Context.Players.FindById(_gameDataSeed.PlayerIds[1]);
        NOT_RELATED_PLAYER = await Context.Players.FindById(_gameDataSeed.PlayerIds[2]);

        /*
         * Give a log to a player
         */
        ANY_PLAYER.GameLogs.Add(new GameLog
        {
            Message = "sdsd",
        });

        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task GivenAnyPlayerWithOneLog_WhenGettingLogs_ThenLogIsVisible()
    {

        GetGameLogsResponse logsResponse = await Mediator.Send(new GetGameLogsRequest
        {
            PlayerId = ANY_PLAYER.Id,
        });
        Assert.True(logsResponse.Logs.Any());
    }

    [Fact]
    public async Task GivenTwoPlayersWithTheSameLog_WhenGettingLogs_ThenLogIsVisibleToBothPlayers()
    {
        GameLog sameLog = ANY_PLAYER.GameLogs.First();
        ANY_OTHER_PLAYER.GameLogs.Add(sameLog);
        await Context.SaveChangesAsync();

        GetGameLogsResponse logsResponse = await Mediator.Send(new GetGameLogsRequest
        {
            PlayerId = ANY_PLAYER.Id,
        });

        Assert.True(logsResponse.Logs.Count == 1);
    }

    [Fact]
    public async Task GivenUnrelatedPlayer_WhenGettingLogs_ThenLogIsInvisibleToBothPlayers()
    {
        GetGameLogsResponse logsResponse = await Mediator.Send(new GetGameLogsRequest
        {
            PlayerId = NOT_RELATED_PLAYER.Id,
        });
        Assert.True(logsResponse.Logs.Count == 0);
    }
}