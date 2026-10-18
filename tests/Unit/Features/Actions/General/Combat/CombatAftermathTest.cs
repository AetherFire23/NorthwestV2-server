using JetBrains.Annotations;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Unit.Dummies;

namespace NorthwestV2.Unit.Features.Actions.General.Combat;

/// <summary>
/// "Information After Combat" of docs/combat.md.
/// </summary>
[TestSubject(typeof(CombatAftermath))]
public class CombatAftermathTest
{
    private const string DEATH_MESSAGE = "You died.";
    private const string HEALTH_REPORT_PREFIX = "After the fight:";

    private readonly CombatAftermath _combatAftermath = new CombatAftermath();

    [Fact]
    public void GivenDefenderDied_WhenCreatingReports_ThenDefenderIsOnlyToldTheyDied()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Dead);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.Equal([DEATH_MESSAGE], MessagesFor(reports, defender));
    }

    [Fact]
    public void GivenDefenderDied_WhenCreatingReports_ThenNoPublicHealthReportIsMade()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Dead);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.DoesNotContain(reports, x => x.Message.StartsWith(HEALTH_REPORT_PREFIX));
    }

    [Fact]
    public void GivenDefenderDied_WhenCreatingReports_ThenTheDeathNoticeGoesToNobodyElse()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Dead);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.Empty(MessagesFor(reports, attacker));
    }

    [Fact]
    public void GivenAttackerDied_WhenCreatingReports_ThenAttackerIsToldTheyDied()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Dead, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.Contains(DEATH_MESSAGE, MessagesFor(reports, attacker));
    }

    [Fact]
    public void GivenDefenderSurvivedAloneWithAttacker_WhenCreatingReports_ThenAttackerSeesBothHealthStates()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        defender.Health = 40;
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.Equal(
            $"{HEALTH_REPORT_PREFIX} the Marine has 100/100 health and the Cook has 40/100 health.",
            Assert.Single(MessagesFor(reports, attacker)));
    }

    [Fact]
    public void GivenDefenderSurvivedAloneWithAttacker_WhenCreatingReports_ThenDefenderSeesTheSameHealthStates()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        defender.Health = 40;
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.Equal(MessagesFor(reports, attacker), MessagesFor(reports, defender));
    }

    [Fact]
    public void GivenDefenderSurvivedAloneWithAttacker_WhenCreatingReports_ThenNothingProvesWhoAttackedFirst()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.DoesNotContain(reports, x => x.Message.Contains("attack"));
    }

    [Fact]
    public void GivenAWitness_WhenCreatingReports_ThenWitnessGetsHardProofOfWhoAttacked()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        Player witness = TestFighters.CreateFighter(12, Roles.Doctor);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, [witness]);

        Assert.Contains("You saw the Marine attack the Cook first.", MessagesFor(reports, witness));
    }

    [Fact]
    public void GivenAWitness_WhenCreatingReports_ThenTheProofIsNotSharedWithTheAttacker()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        Player witness = TestFighters.CreateFighter(12, Roles.Doctor);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, [witness]);

        Assert.DoesNotContain(MessagesFor(reports, attacker), x => x.StartsWith("You saw"));
    }

    [Fact]
    public void GivenTwoWitnesses_WhenCreatingReports_ThenEachWitnessGetsTheirOwnPrivateProof()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        Player firstWitness = TestFighters.CreateFighter(12, Roles.Doctor);
        Player secondWitness = TestFighters.CreateFighter(12, Roles.Captain);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, [firstWitness, secondWitness]);

        Assert.Equal(MessagesFor(reports, firstWitness), MessagesFor(reports, secondWitness));
    }

    [Fact]
    public void GivenAWitnessAndADefenderWhoDied_WhenCreatingReports_ThenWitnessStillGetsTheProof()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        Player witness = TestFighters.CreateFighter(12, Roles.Doctor);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Dead);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, [witness]);

        Assert.Equal(["You saw the Marine attack the Cook first."], MessagesFor(reports, witness));
    }

    [Fact]
    public void GivenAWitnessAndASurvivingDefender_WhenCreatingReports_ThenWitnessAlsoSeesTheHealthStates()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        Player witness = TestFighters.CreateFighter(12, Roles.Doctor);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Alive);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, [witness]);

        Assert.Contains(MessagesFor(reports, witness), x => x.StartsWith(HEALTH_REPORT_PREFIX));
    }

    [Fact]
    public void GivenDefenderWasStunned_WhenCreatingReports_ThenDefenderIsToldTheyWereKnockedOut()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Stunned);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.Contains("You have been knocked out.", MessagesFor(reports, defender));
    }

    [Fact]
    public void GivenDefenderWasStunned_WhenCreatingReports_ThenDefenderIsNotToldTheyDied()
    {
        Player attacker = TestFighters.CreateFighter(12, Roles.Marine);
        Player defender = TestFighters.CreateFighter(12, Roles.Cook);
        FightResult fightResult = CreateFightResult(attacker, defender, PlayerLifeStatus.Alive, PlayerLifeStatus.Stunned);

        List<CombatReport> reports = _combatAftermath.CreateReports(fightResult, []);

        Assert.DoesNotContain(DEATH_MESSAGE, MessagesFor(reports, defender));
    }

    private static FightResult CreateFightResult(Player attacker, Player defender, PlayerLifeStatus attackerStatus,
        PlayerLifeStatus defenderStatus)
    {
        FightResult fightResult = new FightResult
        {
            AttackerFightStats = attacker.GetPlayerTempFightStats(true, AttackerStances.ToTheEnd),
            DefenderFightStats = defender.GetPlayerTempFightStats(false, AttackerStances.ToTheEnd),
            AttackerStatus = attackerStatus,
            DefenderStatus = defenderStatus,
        };

        return fightResult;
    }

    private static List<string> MessagesFor(List<CombatReport> reports, Player player)
    {
        List<string> messages = reports
            .Where(x => x.Recipients.Contains(player))
            .Select(x => x.Message)
            .ToList();

        return messages;
    }
}
