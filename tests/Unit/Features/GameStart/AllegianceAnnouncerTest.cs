using NorthwestV2.Features.GameStart.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.GameStart;

public class AllegianceAnnouncerTest
{
    private const string LoyalMessage = "You are loyal.";

    // "toSpeciyfy" is the default name TestPlayers.CreateTestPlayer gives every player.
    private const string FellowAcolyteMessageForSapper = "You are an acolyte. Your fellow acolyte: toSpeciyfy (Sapper).";

    private const string AcolyteKeyword = "acolyte";
    private const int AnyToughness = 1;

    private readonly Game _game = TestPlayers.CreateTestGame();
    private readonly Room _room;

    public AllegianceAnnouncerTest()
    {
        _room = TestPlayers.CreateTestRoom(RoomEnum.MainDeck, _game);
    }

    [Fact]
    public void GivenLoyalPlayer_WhenAnnouncing_ThenLogSaysLoyal()
    {
        Player loyal = CreatePlayer(Roles.Cook, isAcolyte: false);

        new AllegianceAnnouncer().AnnounceAllegiances([loyal]);

        Assert.Equal(LoyalMessage, loyal.GameLogs.Single().Message);
    }

    [Fact]
    public void GivenTwoAcolytes_WhenAnnouncing_ThenEachLogNamesTheOtherAcolyte()
    {
        Player first = CreatePlayer(Roles.Cook, isAcolyte: true);
        Player second = CreatePlayer(Roles.Sapper, isAcolyte: true);

        new AllegianceAnnouncer().AnnounceAllegiances([first, second]);

        Assert.Equal(FellowAcolyteMessageForSapper,
            first.GameLogs.Single().Message);
    }

    [Fact]
    public void GivenAcolyteAndLoyal_WhenAnnouncing_ThenLoyalLogNeverMentionsAcolytes()
    {
        Player acolyte = CreatePlayer(Roles.Cook, isAcolyte: true);
        Player loyal = CreatePlayer(Roles.Sapper, isAcolyte: false);

        new AllegianceAnnouncer().AnnounceAllegiances([acolyte, loyal]);

        Assert.DoesNotContain(AcolyteKeyword, loyal.GameLogs.Single().Message);
    }

    private Player CreatePlayer(Roles role, bool isAcolyte)
    {
        Player player = TestPlayers.CreateTestPlayer(AnyToughness, role, _room, _game);
        player.IsAcolyte = isAcolyte;

        return player;
    }
}
