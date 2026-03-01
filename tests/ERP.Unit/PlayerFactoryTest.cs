using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;
using NorthwestV2.Integration;

namespace ERP.Testing.Domain;

[TestSubject(typeof(PlayerFactory))]
public class PlayerFactoryTest
{
    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_AllPlayersHaveRoless()
    {
        FakeRandom fakeRandom = new FakeRandom([11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0]);
        PlayerFactory factory = new PlayerFactory(fakeRandom);

        IEnumerable<Player> createdPlayers = factory.CreateFreshPlayersForGame();

        Assert.True(createdPlayers.Count() == GameSettings.RequiredPlayerCountToStartGame);
    }
}