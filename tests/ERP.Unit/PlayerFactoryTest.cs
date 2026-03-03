using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.GameInitialization;
using JetBrains.Annotations;
using NorthwestV2.Integration;

namespace ERP.Testing.Domain;

[TestSubject(typeof(PlayerFactory))]
public class PlayerFactoryTest
{
    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_AllPlayersHaveRoless()
    {
        IEnumerable<int> OPPOSITE_OF_ENUM_GET_VALUES = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0];
        FakeRandom fakeRandom = new FakeRandom(OPPOSITE_OF_ENUM_GET_VALUES.ToList());
        Game fakeGame = new Game();
        IEnumerable<Room> fakeRooms = new RoomFactory().CreateRoomsForGame(fakeGame);
        PlayerFactory factory = new PlayerFactory(fakeRandom);
        List<User> dummyUsers = Enumerable.Range(0, 12)
            .Select(i => new User()
            {
                Username = $"User{i}",
                HashedPassword = "123",
            }).ToList();

        IEnumerable<Player> createdPlayers = factory.CreateFreshPlayersForGame(dummyUsers, fakeGame, fakeRooms);

        Assert.True(createdPlayers.Count() == GameSettings.RequiredPlayerCountToStartGame);
    }
}