using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// "Identification and information" of docs/combat.md.
/// </summary>
[TestSubject(typeof(CombatAftermath))]
public class CombatAftermathTest
{
    private const int NORMAL = ToughnessInitializationConstants.NORMAL;

    private readonly Player _attacker = TestFighters.CreateFighter(NORMAL, Roles.Marine);
    private readonly Player _defender = TestFighters.CreateFighter(NORMAL, Roles.Cook);

    [Fact]
    public void GivenDefenderDied_WhenCreatingReports_ThenDefenderIsOnlyToldHeDied()
    {
        FightResult fightResult = CreateFightResult(FightExitType.Knockout, defenderStatus: PlayerLifeStatus.Dead);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, []);

        Assert.Equal([CombatAftermath.DEATH_MESSAGE], MessagesFor(reports, _defender));
    }

    [Fact]
    public void GivenAttackerDied_WhenCreatingReports_ThenAttackerIsOnlyToldHeDied()
    {
        FightResult fightResult = CreateFightResult(FightExitType.Knockout, attackerStatus: PlayerLifeStatus.Dead);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, []);

        Assert.Equal([CombatAftermath.DEATH_MESSAGE], MessagesFor(reports, _attacker));
    }

    [Fact]
    public void GivenDefenderSurvivedAFullCombat_WhenCreatingReports_ThenHeIdentifiesTheAttacker()
    {
        FightResult fightResult = CreateFightResult(FightExitType.Knockout, attackerStatus: PlayerLifeStatus.Dead);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, []);

        Assert.Equal(["The Marine attacked you."], MessagesFor(reports, _defender));
    }

    [Fact]
    public void GivenDefenderStunned_WhenCreatingReports_ThenHeIdentifiesTheAttackerAndKnowsHeWasKnockedOut()
    {
        FightResult fightResult = CreateFightResult(FightExitType.Knockout, defenderStatus: PlayerLifeStatus.Stunned);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, []);

        Assert.Equal(["The Marine attacked you. You have been knocked out."], MessagesFor(reports, _defender));
    }

    [Theory]
    [InlineData(true, 19, "The Marine targeted you with a hit-and-run.")]
    [InlineData(true, 20, "Someone targeted you with a hit-and-run. You could not see who.")]
    [InlineData(false, 59, "The Marine targeted you with a hit-and-run.")]
    [InlineData(false, 60, "Someone targeted you with a hit-and-run. You could not see who.")]
    public void GivenHitAndRunStoppedEarly_WhenCreatingReports_ThenDefenderIdentifiesTheAttackerOnlyOnALuckyRoll(
        bool wasSurprised, int roll, string expected)
    {
        FightResult fightResult = CreateFightResult(FightExitType.StoppedByHitAndRun, wasSurprised: wasSurprised);

        List<CombatReport> reports = CreateAftermath(roll).CreateReports(fightResult, []);

        Assert.Equal([expected], MessagesFor(reports, _defender));
    }

    [Fact]
    public void GivenAttackerBrokeOff_WhenCreatingReports_ThenAttackerIsToldHeBrokeOff()
    {
        FightResult fightResult = CreateFightResult(FightExitType.BrokeOffUnderPushHard);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, []);

        Assert.Equal(["You attacked the Cook. You broke off the attack."], MessagesFor(reports, _attacker));
    }

    [Fact]
    public void GivenWitness_WhenCreatingReports_ThenHeSeesWhoStartedItAndTheWeaponsUsed()
    {
        Player witness = TestFighters.CreateFighter(NORMAL, Roles.Doctor);
        FightResult fightResult = CreateFightResult(FightExitType.Knockout, defenderStatus: PlayerLifeStatus.Dead,
            attackerWeapons: [Weapons.Musket, Weapons.Unarmed]);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, [witness]);

        Assert.Equal(["You saw the Marine attack the Cook. The Marine fought with a musket and bare hands. " +
                      "The Cook did not strike."], MessagesFor(reports, witness));
    }

    [Fact]
    public void GivenWitness_WhenCreatingReports_ThenTheProofReachesNoFighter()
    {
        Player witness = TestFighters.CreateFighter(NORMAL, Roles.Doctor);
        FightResult fightResult = CreateFightResult(FightExitType.Knockout, defenderStatus: PlayerLifeStatus.Dead);

        List<CombatReport> reports = CreateAftermath().CreateReports(fightResult, [witness]);

        Assert.DoesNotContain(reports, x => x.Message.StartsWith("You saw") && x.Recipients.Contains(_attacker));
    }

    private static CombatAftermath CreateAftermath(int roll = 0)
    {
        return new CombatAftermath(new FakeRandom([roll]));
    }

    private FightResult CreateFightResult(FightExitType exitType,
        PlayerLifeStatus attackerStatus = PlayerLifeStatus.Alive,
        PlayerLifeStatus defenderStatus = PlayerLifeStatus.Alive,
        bool wasSurprised = false,
        List<Weapons>? attackerWeapons = null)
    {
        return new FightResult
        {
            Attacker = CreateCombatant(_attacker, attackerWeapons ?? [Weapons.Unarmed]),
            Defender = CreateCombatant(_defender, []),
            Stance = AttackerStances.PushHard,
            ExitType = exitType,
            WasDefenderSurprised = wasSurprised,
            AttackerStatus = attackerStatus,
            DefenderStatus = defenderStatus,
        };
    }

    private static Combatant CreateCombatant(Player player, List<Weapons> weaponsUsed)
    {
        return new Combatant
        {
            Player = player,
            Toughness = player.BaseToughness,
            RangedWeapon = null,
            MeleeWeapon = Weapons.Unarmed,
            RangedBasicDamage = 0,
            MeleeBasicDamage = 0,
            RangedModifierPercent = 0,
            MeleeModifierPercent = 0,
            DamageDealt = AttackTypes.Kill,
            Health = player.Health,
            WeaponsUsed = weaponsUsed,
        };
    }

    private static List<string> MessagesFor(List<CombatReport> reports, Player player)
    {
        return reports.Where(x => x.Recipients.Contains(player)).Select(x => x.Message).ToList();
    }
}
