using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Decides who learns what once a fight is over (docs/combat.md, "Information After Combat").
/// </summary>
public class CombatAftermath
{
    /// <summary>
    /// - A combatant who went down for good is told only that they died.
    /// - If the defender did not die, the post-combat Health of both is shown to the combatants and the witnesses.
    ///   Nothing says who attacked first, so either can claim self-defense.
    /// - Witnesses privately get hard proof of who attacked. Nobody else gets it.
    /// - If the defender died, no state report is made: the attacker controls the narrative.
    /// </summary>
    public List<CombatReport> CreateReports(FightResult fightResult, List<Player> witnesses)
    {
        Player attacker = fightResult.AttackerFightStats.Player;
        Player defender = fightResult.DefenderFightStats.Player;

        List<CombatReport> reports = [];

        AddDeathNotice(reports, attacker, fightResult.AttackerStatus);
        AddDeathNotice(reports, defender, fightResult.DefenderStatus);
        AddStunNotice(reports, attacker, fightResult.AttackerStatus);
        AddStunNotice(reports, defender, fightResult.DefenderStatus);

        bool hasDefenderDied = fightResult.DefenderStatus == PlayerLifeStatus.Dead;
        if (!hasDefenderDied)
        {
            reports.Add(CreatePublicHealthReport(attacker, defender, witnesses));
        }

        if (witnesses.Count > 0)
        {
            reports.Add(CreateWitnessProof(attacker, defender, witnesses));
        }

        return reports;
    }

    private static void AddDeathNotice(List<CombatReport> reports, Player player, PlayerLifeStatus status)
    {
        if (status != PlayerLifeStatus.Dead)
        {
            return;
        }

        reports.Add(new CombatReport
        {
            Message = "You died.",
            Recipients = [player],
        });
    }

    private static void AddStunNotice(List<CombatReport> reports, Player player, PlayerLifeStatus status)
    {
        if (status != PlayerLifeStatus.Stunned)
        {
            return;
        }

        reports.Add(new CombatReport
        {
            Message = "You have been knocked out.",
            Recipients = [player],
        });
    }

    private static CombatReport CreatePublicHealthReport(Player attacker, Player defender, List<Player> witnesses)
    {
        List<Player> recipients = [attacker, defender, ..witnesses];

        CombatReport report = new CombatReport
        {
            Message = $"After the fight: {DescribeHealth(attacker)} and {DescribeHealth(defender)}.",
            Recipients = recipients,
        };

        return report;
    }

    private static CombatReport CreateWitnessProof(Player attacker, Player defender, List<Player> witnesses)
    {
        CombatReport report = new CombatReport
        {
            Message = $"You saw the {attacker.Role} attack the {defender.Role} first.",
            Recipients = witnesses,
        };

        return report;
    }

    private static string DescribeHealth(Player player)
    {
        return $"the {player.Role} has {player.Health}/{Player.INITIALIZATION_HEALTH} health";
    }
}
