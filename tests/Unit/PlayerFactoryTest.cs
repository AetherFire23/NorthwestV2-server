using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NorthwestV2.Features;
using NorthwestV2.Features.GameStart.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using Xunit.Abstractions;

namespace NorthwestV2.Unit;

[TestSubject(typeof(PlayerFactory))]
public class PlayerFactoryTest
{
    private const string AnyHashedPassword = "123";

    private static readonly List<int> ReversedRoleShuffle = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0];

    // Chaplain (enum index 1) comes first in the shuffled roles, so it would be picked if it were eligible.
    private static readonly List<int> ChaplainFirstShuffle = [1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

    private readonly ITestOutputHelper _outputHelper;

    public PlayerFactoryTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_ThenPlayerCountIsCorrect()
    {
        FakeRandom fakeRandom = new FakeRandom(ReversedRoleShuffle);
        Game fakeGame = new Game();
        IEnumerable<Room> fakeRooms = new RoomFactory().CreateRoomsForGame(fakeGame);
        var roleInitalizers = ScanRoleInitalizers();
        ILogger<PlayerFactory> lg = new TestOutputLogger<PlayerFactory>(_outputHelper);
        PlayerFactory factory = new PlayerFactory(fakeRandom, roleInitalizers, lg);
        List<User> dummyUsers = Enumerable.Range(0, GameSettings.RequiredPlayerCountToStartGame)
            .Select(i => new User()
            {
                Username = $"User{i}",
                HashedPassword = AnyHashedPassword,
            }).ToList();


        IEnumerable<Player> createdPlayers = factory.CreateFreshPlayersForGame(dummyUsers, fakeGame, fakeRooms);
        Assert.Equal(GameSettings.RequiredPlayerCountToStartGame, createdPlayers.Count());
    }

    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_ThenAllPlayerAreNotNull()
    {
        FakeRandom fakeRandom = new FakeRandom(ReversedRoleShuffle);
        Game fakeGame = new Game();
        IEnumerable<Room> fakeRooms = new RoomFactory().CreateRoomsForGame(fakeGame);
        var roleInitalizers = ScanRoleInitalizers();

        ILogger<PlayerFactory> lg = new TestOutputLogger<PlayerFactory>(_outputHelper);
        PlayerFactory factory = new PlayerFactory(fakeRandom, roleInitalizers, lg);
        List<User> dummyUsers = Enumerable.Range(0, GameSettings.RequiredPlayerCountToStartGame)
            .Select(i => new User()
            {
                Username = $"User{i}",
                HashedPassword = AnyHashedPassword,
            }).ToList();


        IEnumerable<Player> createdPlayers = factory.CreateFreshPlayersForGame(dummyUsers, fakeGame, fakeRooms);

        Assert.All(createdPlayers, Assert.NotNull);
    }

    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_ThenExactlyTwoAcolytes()
    {
        IEnumerable<Player> createdPlayers = CreatePlayersWithShuffle(ReversedRoleShuffle);

        Assert.Equal(AllegianceConstants.AcolyteCount, createdPlayers.Count(x => x.IsAcolyte));
    }

    [Fact]
    public void GivenChaplainShuffledFirst_WhenCreatePlayers_ThenChaplainIsNotAnAcolyte()
    {
        IEnumerable<Player> createdPlayers = CreatePlayersWithShuffle(ChaplainFirstShuffle);

        Assert.False(createdPlayers.Single(x => x.Role == Roles.Chaplain).IsAcolyte);
    }

    private IEnumerable<Player> CreatePlayersWithShuffle(List<int> shuffle)
    {
        FakeRandom fakeRandom = new FakeRandom(shuffle);
        Game fakeGame = new Game();
        IEnumerable<Room> fakeRooms = new RoomFactory().CreateRoomsForGame(fakeGame);
        ILogger<PlayerFactory> lg = new TestOutputLogger<PlayerFactory>(_outputHelper);
        PlayerFactory factory = new PlayerFactory(fakeRandom, ScanRoleInitalizers(), lg);
        List<User> dummyUsers = Enumerable.Range(0, GameSettings.RequiredPlayerCountToStartGame)
            .Select(i => new User()
            {
                Username = $"User{i}",
                HashedPassword = AnyHashedPassword,
            }).ToList();

        return factory.CreateFreshPlayersForGame(dummyUsers, fakeGame, fakeRooms).ToList();
    }

    private IEnumerable<RoleInitializer> ScanRoleInitalizers()
    {
        var roleInitalizers = typeof(DomainInstaller).Assembly.GetTypes()
            .Where(t => typeof(RoleInitializer).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .Select(r => (RoleInitializer)Activator.CreateInstance(r));

        return roleInitalizers;
    }
}