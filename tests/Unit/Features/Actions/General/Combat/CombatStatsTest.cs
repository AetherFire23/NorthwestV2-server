using JetBrains.Annotations;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// "Combatant statistics" of docs/combat.md: toughness per role, starting Health and starting weapons.
/// </summary>
[TestSubject(typeof(Player))]
public class CombatStatsTest
{
    [Theory]
    [InlineData(Roles.Chaplain, ToughnessInitializationConstants.WEAK)]
    [InlineData(Roles.Scholar, ToughnessInitializationConstants.WEAK)]
    [InlineData(Roles.Doctor, ToughnessInitializationConstants.WEAK)]
    [InlineData(Roles.Marine, ToughnessInitializationConstants.TOUGH)]
    [InlineData(Roles.Brute, ToughnessInitializationConstants.TOUGH)]
    [InlineData(Roles.Sapper, ToughnessInitializationConstants.TOUGH)]
    [InlineData(Roles.Captain, ToughnessInitializationConstants.NORMAL)]
    [InlineData(Roles.QuarterMaster, ToughnessInitializationConstants.NORMAL)]
    [InlineData(Roles.Sentry, ToughnessInitializationConstants.NORMAL)]
    [InlineData(Roles.Cook, ToughnessInitializationConstants.NORMAL)]
    [InlineData(Roles.Engineer, ToughnessInitializationConstants.NORMAL)]
    [InlineData(Roles.Ranger, ToughnessInitializationConstants.NORMAL)]
    public void GivenRole_WhenInitializingThePlayer_ThenBaseToughnessMatchesTheRoleTier(Roles role, int expectedToughness)
    {
        RoleInitializer initializer = FindInitializerOf(role);
        RoleInitializationContext context = CreateContext();

        Player player = initializer.CreateAndInitializePlayer(context);

        Assert.Equal(expectedToughness, player.BaseToughness);
    }

    [Fact]
    public void GivenAnyNewPlayer_WhenInitializing_ThenHealthIsFull()
    {
        RoleInitializer initializer = FindInitializerOf(Roles.Captain);
        RoleInitializationContext context = CreateContext();

        Player player = initializer.CreateAndInitializePlayer(context);

        Assert.Equal(100, player.Health);
    }

    [Theory]
    [InlineData(Roles.Captain, new[] { ItemTypes.Sword, ItemTypes.Pistol })]
    [InlineData(Roles.Marine, new[] { ItemTypes.Sword, ItemTypes.Musket })]
    [InlineData(Roles.Sapper, new[] { ItemTypes.Pistol })]
    [InlineData(Roles.Ranger, new[] { ItemTypes.Knife, ItemTypes.Musket })]
    [InlineData(Roles.Cook, new[] { ItemTypes.Knife })]
    [InlineData(Roles.Engineer, new ItemTypes[0])]
    public void GivenRole_WhenInitializingThePlayer_ThenStartingWeaponsMatchTheRole(Roles role, ItemTypes[] expected)
    {
        RoleInitializer initializer = FindInitializerOf(role);
        RoleInitializationContext context = CreateContext();

        Player player = initializer.CreateAndInitializePlayer(context);

        Assert.Equal(expected, player.Inventory.Items.Select(x => x.ItemType));
    }

    private static RoleInitializer FindInitializerOf(Roles role)
    {
        RoleInitializer initializer = typeof(RoleInitializer).Assembly.GetTypes()
            .Where(t => typeof(RoleInitializer).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => (RoleInitializer)Activator.CreateInstance(t)!)
            .Single(x => x.Role == role);

        return initializer;
    }

    private static RoleInitializationContext CreateContext()
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);

        RoleInitializationContext context = new RoleInitializationContext
        {
            Game = game,
            Rooms = [room],
            User = new User { HashedPassword = "noneed", Username = "toSpecify" },
        };

        return context;
    }
}
