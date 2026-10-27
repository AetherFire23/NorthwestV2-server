using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.General.Combat.Items;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// "Order of a combat" and "End of combat" of docs/combat.md. Expected values are worked out by hand in each test.
/// Unarmed damage is Toughness x 5: an Engineer (Toughness 4) hits for 20, a Brute (5) for 40, a Chaplain (3) for 15.
/// </summary>
[TestSubject(typeof(CombatResolver))]
public class CombatResolverTest
{
    private const int WEAK = ToughnessInitializationConstants.WEAK;
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;
    private const int TOUGH = ToughnessInitializationConstants.TOUGH;

    private readonly CombatAction _combatAction = new CombatAction(new CombatSetupFactory(), new CombatResolver());

    /*
     * Rounds
     */

    // Attacker strikes 22 (+10%) on Toughness 4: 5 then 6 Health a round. Defender strikes 20: 5 a round.
    // The defender goes down on the attacker's 19th strike, after striking back 18 times.
    [Fact]
    public void GivenEqualUnarmedFightersToTheEnd_WhenFighting_ThenDefenderDies()
    {
        Player attacker = Engineer();
        Player defender = Engineer().Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.ToTheEnd));

        Assert.Equal(PlayerLifeStatus.Dead, defender.LifeStatus);
    }

    [Fact]
    public void GivenEqualUnarmedFightersToTheEnd_WhenFighting_ThenAttackerKeepsTheHealthOfOneStrikeLess()
    {
        Player attacker = Engineer();
        Player defender = Engineer().Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.ToTheEnd));

        Assert.Equal(10, attacker.Health);
    }

    [Fact]
    public void GivenStrongerDefenderToTheEnd_WhenFighting_ThenAttackerDies()
    {
        Player attacker = Engineer();
        Player defender = Brute().Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.ToTheEnd));

        Assert.Equal(PlayerLifeStatus.Dead, attacker.LifeStatus);
    }

    // The Brute takes 10 Health a round from the Engineer: at 20 Health, after round 8, the Engineer would lose.
    [Fact]
    public void GivenStrongerDefenderAndPushHard_WhenFighting_ThenAttackerBreaksOff()
    {
        Player attacker = Engineer();
        Player defender = Brute().Vigilant();

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.PushHard));

        Assert.Equal(FightExitType.BrokeOffUnderPushHard, result.ExitType);
    }

    [Fact]
    public void GivenStrongerDefenderAndPushHard_WhenFighting_ThenAttackerBreaksOffAtTwentyHealth()
    {
        Player attacker = Engineer();
        Player defender = Brute().Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.PushHard));

        Assert.Equal(20, attacker.Health);
    }

    [Fact]
    public void GivenAttackerWithoutStrength_WhenReachingTheRounds_ThenHeBreaksOff()
    {
        Player attacker = Engineer();
        attacker.Health = 10;
        attacker.Hope = 10;
        attacker.IsWeakened = true;
        Player defender = Engineer().Vigilant();

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.PushHard, AttackTypes.Stun));

        Assert.Equal(FightExitType.BrokeOffWithoutStrength, result.ExitType);
    }

    /*
     * Surprise attack
     */

    // Brute surprise in melee: 40 x 2 = 80 on Toughness 3 takes 26 Health, more than the 20 left.
    [Fact]
    public void GivenSurpriseAttackThatDownsTheDefender_WhenFighting_ThenDefenderNeverStrikes()
    {
        Player attacker = Brute();
        Player defender = Chaplain();
        defender.Health = 20;

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.PushHard));

        Assert.Empty(result.Defender.WeaponsUsed);
    }

    /*
     * Hit-and-run
     */

    [Fact]
    public void GivenVigilantStrongerDefenderAndHitAndRun_WhenFighting_ThenCombatStopsEarly()
    {
        Player attacker = Engineer();
        Player defender = Brute().Vigilant();

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.HitAndRun));

        Assert.Equal(FightExitType.StoppedByHitAndRun, result.ExitType);
    }

    [Fact]
    public void GivenVigilantStrongerDefenderAndHitAndRun_WhenFighting_ThenNoDamageIsDealt()
    {
        Player attacker = Engineer();
        Player defender = Brute().Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.HitAndRun));

        Assert.Equal(100, defender.Health);
    }

    // Melee surprise: 20 x 2 = 40 on Toughness 5 takes 8 Health, then the hit-and-run stops.
    [Fact]
    public void GivenSurprisedStrongerDefenderAndMeleeHitAndRun_WhenFighting_ThenOnlyTheDoubledSurpriseLands()
    {
        Player attacker = Engineer();
        Player defender = Brute();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.HitAndRun));

        Assert.Equal(92, defender.Health);
    }

    // Ranged surprise is not doubled: the pistol's 100 on Toughness 5 takes 20 Health.
    [Fact]
    public void GivenSurprisedStrongerDefenderAndPistolHitAndRun_WhenFighting_ThenSurpriseIsNotDoubled()
    {
        Player attacker = Engineer().Carrying(new Pistol());
        Player defender = Brute();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.HitAndRun, rangedWeapon: Weapons.Pistol));

        Assert.Equal(80, defender.Health);
    }

    // Brute at 32 (-20%) takes ~10.7 Health a round from the Chaplain, who only takes 3 back: the Brute keeps 73.
    [Fact]
    public void GivenWeakerDefenderAndHitAndRun_WhenFighting_ThenCombatGoesAheadInFull()
    {
        Player attacker = Brute();
        Player defender = Chaplain().Vigilant();

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.HitAndRun));

        Assert.Equal(FightExitType.Knockout, result.ExitType);
    }

    /*
     * Closing in
     */

    // The unarmed defender closes in and takes the pistol's 100 (25 Health), then loses the melee rounds.
    [Fact]
    public void GivenRangedAttackerAndUnarmedDefender_WhenFighting_ThenAttackerShootsThenFightsInMelee()
    {
        Player attacker = Engineer().Carrying(new Pistol());
        Player defender = Engineer().Vigilant();

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.ToTheEnd, rangedWeapon: Weapons.Pistol));

        Assert.Equal([Weapons.Pistol, Weapons.Unarmed], result.Attacker.WeaponsUsed);
    }

    [Fact]
    public void GivenRangedAttackerAndUnarmedDefender_WhenFighting_ThenClosingInCostsTheDefenderAStrike()
    {
        Player attacker = Engineer().Carrying(new Pistol());
        Player defender = Engineer().Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.ToTheEnd, rangedWeapon: Weapons.Pistol));

        Assert.Equal(35, attacker.Health);
    }

    // Outside. Ranger musket 200 (+10% = 220) vs Engineer pistol 100, both Toughness 4.
    // Ranged rounds: the Engineer dies and leaves the Ranger at 75. Closing in, the Engineer takes the musket's 200
    // (50 Health) but leaves the Ranger at 55 after the melee: losing either way, he closes in.
    [Fact]
    public void GivenBothRangedAndDefenderLosesLessByClosingIn_WhenFighting_ThenDefenderClosesIn()
    {
        Player attacker = TestFighters.CreateFighterIn(RoomEnum.MainDeck, NORMAL, Roles.Ranger).Carrying(new Musket());
        Player defender = Engineer().Carrying(new Pistol()).Vigilant();

        FightResult result = _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.ToTheEnd, rangedWeapon: Weapons.Musket));

        Assert.Equal([Weapons.Unarmed], result.Defender.WeaponsUsed);
    }

    [Fact]
    public void GivenBothRangedAndDefenderLosesLessByClosingIn_WhenFighting_ThenAttackerKeepsLessHealth()
    {
        Player attacker = TestFighters.CreateFighterIn(RoomEnum.MainDeck, NORMAL, Roles.Ranger).Carrying(new Musket());
        Player defender = Engineer().Carrying(new Pistol()).Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.ToTheEnd, rangedWeapon: Weapons.Musket));

        Assert.Equal(55, attacker.Health);
    }

    // Outside. Marine (Toughness 5) musket 200 vs Ranger (Toughness 4) musket 200. Ranged rounds leave the Marine at 60.
    // Closing in would leave the Marine at 4 (his choice) or at 72 (the Ranger's choice): nobody closes in.
    [Fact]
    public void GivenBothRangedAndNeitherGainsByClosingIn_WhenFighting_ThenRoundsStayRanged()
    {
        Player attacker = TestFighters.CreateFighterIn(RoomEnum.MainDeck, TOUGH, Roles.Marine).Carrying(new Musket());
        Player defender = TestFighters.CreateFighter(NORMAL, Roles.Ranger).Carrying(new Musket()).Vigilant();

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.ToTheEnd, rangedWeapon: Weapons.Musket));

        Assert.Equal(60, attacker.Health);
    }

    /*
     * Stun damage and end of combat
     */

    // Both stun for 16 (-20%): 4 Health a strike. The defender (99) goes down on the 25th strike.
    [Fact]
    public void GivenStunAttack_WhenDefenderReachesZero_ThenHeIsStunned()
    {
        Player attacker = Engineer();
        Player defender = Engineer().Vigilant();
        defender.Health = 99;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.PushHard, AttackTypes.Stun));

        Assert.Equal(PlayerLifeStatus.Stunned, defender.LifeStatus);
    }

    // 99 Health lost to stun: 74.25 is given back, rounded up to 75.
    [Fact]
    public void GivenStunAttack_WhenCombatEnds_ThenThreeQuartersOfTheStunLossIsGivenBackRoundedUp()
    {
        Player attacker = Engineer();
        Player defender = Engineer().Vigilant();
        defender.Health = 99;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.PushHard, AttackTypes.Stun));

        Assert.Equal(75, defender.Health);
    }

    // The attacker took 24 stun strikes of 4: 96 lost, 72 given back.
    [Fact]
    public void GivenStunAttack_WhenCombatEnds_ThenTheWinnerAlsoGetsHisStunLossBack()
    {
        Player attacker = Engineer();
        Player defender = Engineer().Vigilant();
        defender.Health = 99;

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender,
            TestFighters.Choices(AttackerStances.PushHard, AttackTypes.Stun));

        Assert.Equal(76, attacker.Health);
    }

    [Fact]
    public void GivenKillAttackAndStunCounter_WhenAttackerLoses_ThenAttackerIsOnlyStunned()
    {
        Player attacker = Engineer();
        Player defender = Brute().Vigilant();
        defender.SetCombatReaction(attacker.Id, AttackTypes.Kill, AttackTypes.Stun, WeaponPolicies.StrongestOwned,
            FakedHopePenalties.None);

        _combatAction.MakeTwoPlayerFightTogether(attacker, defender, TestFighters.Choices(AttackerStances.ToTheEnd));

        Assert.Equal(PlayerLifeStatus.Stunned, attacker.LifeStatus);
    }

    private static Player Engineer()
    {
        return TestFighters.CreateFighter(NORMAL);
    }

    private static Player Brute()
    {
        return TestFighters.CreateFighter(TOUGH, Roles.Brute);
    }

    private static Player Chaplain()
    {
        return TestFighters.CreateFighter(WEAK, Roles.Chaplain);
    }
}
