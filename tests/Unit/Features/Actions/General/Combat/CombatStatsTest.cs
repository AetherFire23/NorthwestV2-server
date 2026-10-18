using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// "Combat Core" stats and "Hidden Modifiers" of docs/combat.md: toughness per role, unarmed Strength, sanity.
/// </summary>
[TestSubject(typeof(Player))]
public class CombatStatsTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;

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

    [Fact]
    public void GivenToughFighterAndWeakFighter_WhenCalculatingUnarmedStrength_ThenToughFighterHitsHarder()
    {
        Player tough = TestFighters.CreateFighter(ToughnessInitializationConstants.TOUGH);
        Player weak = TestFighters.CreateFighter(ToughnessInitializationConstants.WEAK);

        bool isToughHarder = tough.CalculateStrength(true, AttackerStances.PushHard)
                             > weak.CalculateStrength(true, AttackerStances.PushHard);

        Assert.True(isToughHarder);
    }

    [Fact]
    public void GivenUnarmedFighterWithFullSanity_WhenCalculatingStrength_ThenStrengthIsTheirToughness()
    {
        Player fighter = TestFighters.CreateFighter(ToughnessInitializationConstants.TOUGH);

        int strength = fighter.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(ToughnessInitializationConstants.TOUGH, strength);
    }

    [Fact]
    public void GivenDepressedFighter_WhenCalculatingStrength_ThenStrengthIsWeakerThanAHealthyMind()
    {
        Player depressed = TestFighters.CreateFighter(NORMAL);
        depressed.Sanity = 10;
        Player healthy = TestFighters.CreateFighter(NORMAL);

        bool isDepressedWeaker = depressed.CalculateStrength(true, AttackerStances.PushHard)
                                 < healthy.CalculateStrength(true, AttackerStances.PushHard);

        Assert.True(isDepressedWeaker);
    }

    [Fact]
    public void GivenDepressedFighterWithAToughnessOfOne_WhenCalculatingStrength_ThenStrengthNeverDropsBelowOne()
    {
        Player depressed = TestFighters.CreateFighter(1);
        depressed.Sanity = 0;

        int strength = depressed.CalculateStrength(true, AttackerStances.PushHard);

        Assert.Equal(1, strength);
    }

    [Fact]
    public void GivenDepressedDefender_WhenCalculatingStrength_ThenDefenderIsAlsoWeaker()
    {
        Player depressed = TestFighters.CreateFighter(NORMAL);
        depressed.Sanity = 10;

        int strength = depressed.CalculateStrength(false, AttackerStances.PushHard);

        Assert.True(strength < NORMAL);
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
