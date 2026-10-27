using JetBrains.Annotations;
using NorthwestV2.Features.Actions.General.Combat.Items;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// Weapon table and specialist bonuses of docs/combat.md, "Strength".
/// </summary>
[TestSubject(typeof(WeaponCatalog))]
public class WeaponCatalogTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;
    private const int TOUGH = ToughnessInitializationConstants.TOUGH;

    [Theory]
    [InlineData(Weapons.Knife, false, 40)]
    [InlineData(Weapons.Knife, true, 40)]
    [InlineData(Weapons.Sword, false, 30)]
    [InlineData(Weapons.Sword, true, 60)]
    [InlineData(Weapons.Pistol, false, 100)]
    [InlineData(Weapons.Pistol, true, 100)]
    [InlineData(Weapons.Musket, false, 75)]
    [InlineData(Weapons.Musket, true, 175)]
    public void GivenNonSpecialist_WhenGettingBasicDamage_ThenMatchesTheWeaponTable(Weapons weapon, bool isOutside,
        int expected)
    {
        Player engineer = TestFighters.CreateFighter(NORMAL);

        decimal damage = WeaponCatalog.BasicDamage(engineer, weapon, isOutside);

        Assert.Equal(expected, damage);
    }

    [Fact]
    public void GivenUnarmedFighter_WhenGettingBasicDamage_ThenItIsFiveTimesHisToughness()
    {
        Player engineer = TestFighters.CreateFighter(NORMAL);

        decimal damage = WeaponCatalog.BasicDamage(engineer, Weapons.Unarmed, false);

        Assert.Equal(20, damage);
    }

    [Theory]
    [InlineData(Roles.Brute, Weapons.Unarmed, false, 40)]
    [InlineData(Roles.Marine, Weapons.Sword, true, 75)]
    [InlineData(Roles.Marine, Weapons.Musket, true, 200)]
    [InlineData(Roles.Ranger, Weapons.Musket, false, 100)]
    [InlineData(Roles.Marine, Weapons.Unarmed, false, 25)]
    public void GivenSpecialist_WhenGettingBasicDamage_ThenWeaponOfChoiceGetsTheBonus(Roles role, Weapons weapon,
        bool isOutside, int expected)
    {
        Player specialist = TestFighters.CreateFighter(TOUGH, role);

        decimal damage = WeaponCatalog.BasicDamage(specialist, weapon, isOutside);

        Assert.Equal(expected, damage);
    }

    [Fact]
    public void GivenConcealedPistol_WhenListingWeaponsNotConcealed_ThenPistolIsLeftOut()
    {
        Player fighter = TestFighters.CreateFighter(NORMAL).Carrying(new Pistol { IsConcealed = true });

        List<Weapons> owned = WeaponCatalog.OwnedWeapons(fighter, WeaponRanges.Ranged, includeConcealed: false);

        Assert.Empty(owned);
    }

    [Fact]
    public void GivenKnifeAndSwordInside_WhenPickingTheStrongest_ThenKnifeWins()
    {
        Player fighter = TestFighters.CreateFighter(NORMAL);

        Weapons strongest = WeaponCatalog.Strongest(fighter, [Weapons.Unarmed, Weapons.Sword, Weapons.Knife], false);

        Assert.Equal(Weapons.Knife, strongest);
    }

    [Fact]
    public void GivenBruteWithKnife_WhenPickingTheStrongest_ThenTieKeepsBareHands()
    {
        Player brute = TestFighters.CreateFighter(TOUGH, Roles.Brute);

        Weapons strongest = WeaponCatalog.Strongest(brute, [Weapons.Unarmed, Weapons.Knife], false);

        Assert.Equal(Weapons.Unarmed, strongest);
    }
}
