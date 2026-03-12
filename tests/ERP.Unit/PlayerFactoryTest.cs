using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameStart;
using AetherFire23.ERP.Domain.GameStart.RoleInitializations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NorthwestV2.Integration;
using Xunit.Abstractions;

namespace ERP.Testing.Domain;

[TestSubject(typeof(PlayerFactory))]
public class PlayerFactoryTest
{

    private readonly ITestOutputHelper _outputHelper;

    public PlayerFactoryTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_ThenPlayerCountIsCorrect()
    {
        IEnumerable<int> OPPOSITE_OF_ENUM_GET_VALUES = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0];
        FakeRandom fakeRandom = new FakeRandom(OPPOSITE_OF_ENUM_GET_VALUES.ToList());
        Game fakeGame = new Game();
        IEnumerable<Room> fakeRooms = new RoomFactory().CreateRoomsForGame(fakeGame);
        var roleInitalizers = ScanRoleInitalizers();
        ILogger<PlayerFactory> lg = new TestOutputLogger<PlayerFactory>(_outputHelper);
        PlayerFactory factory = new PlayerFactory(fakeRandom, roleInitalizers, lg);
        List<User> dummyUsers = Enumerable.Range(0, 12)
            .Select(i => new User()
            {
                Username = $"User{i}",
                HashedPassword = "123",
            }).ToList();


        IEnumerable<Player> createdPlayers = factory.CreateFreshPlayersForGame(dummyUsers, fakeGame, fakeRooms);
        Assert.Equal(GameSettings.RequiredPlayerCountToStartGame, createdPlayers.Count());
    }

    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_ThenAllPlayerAreNotNull()
    {
        IEnumerable<int> OPPOSITE_OF_ENUM_GET_VALUES = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0];
        FakeRandom fakeRandom = new FakeRandom(OPPOSITE_OF_ENUM_GET_VALUES.ToList());
        Game fakeGame = new Game();
        IEnumerable<Room> fakeRooms = new RoomFactory().CreateRoomsForGame(fakeGame);
        var roleInitalizers = ScanRoleInitalizers();

        ILogger<PlayerFactory> lg = new TestOutputLogger<PlayerFactory>(_outputHelper);
        PlayerFactory factory = new PlayerFactory(fakeRandom, roleInitalizers, lg);
        List<User> dummyUsers = Enumerable.Range(0, 12)
            .Select(i => new User()
            {
                Username = $"User{i}",
                HashedPassword = "123",
            }).ToList();


        IEnumerable<Player> createdPlayers = factory.CreateFreshPlayersForGame(dummyUsers, fakeGame, fakeRooms);

        Assert.All(createdPlayers, Assert.NotNull);
    }

    private IEnumerable<RoleInitializer> ScanRoleInitalizers()
    {
        var roleInitalizers = typeof(DomainInstaller).Assembly.GetTypes()
            .Where(t => typeof(RoleInitializer).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .Select(r => (RoleInitializer)Activator.CreateInstance(r));

        return roleInitalizers;
    }
}