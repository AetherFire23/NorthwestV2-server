using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Decides who learns what once a fight is over (docs/combat.md, "Identification and information").
/// Every report is private. The only public trace of a combat is the change in Health increments, which other
/// players read from the player list, not from logs.
/// </summary>
public class CombatAftermath
{
    public const string DEATH_MESSAGE = "You died.";

    private readonly IRandomProvider _randomProvider;

    public CombatAftermath(IRandomProvider randomProvider)
    {
        _randomProvider = randomProvider;
    }

    /// <summary>
    /// - A dead fighter is only told that he died.
    /// - A defender who was not killed identifies the attacker, except after a hit-and-run that stopped early,
    ///   where only a random roll does (he always knows he was targeted).
    /// - The attacker, unless dead, learns how it ended for the defender.
    /// - Witnesses learn who started the combat and which weapons each fighter struck with.
    /// </summary>
    public List<CombatReport> CreateReports(FightResult fightResult, List<Player> witnesses)
    {
        Player attacker = fightResult.Attacker.Player;
        Player defender = fightResult.Defender.Player;

        List<CombatReport> reports = [];

        reports.Add(CreateDefenderReport(fightResult, attacker, defender));

        reports.Add(fightResult.AttackerStatus == PlayerLifeStatus.Dead
            ? new CombatReport { Message = DEATH_MESSAGE, Recipients = [attacker] }
            : CreateAttackerReport(fightResult, attacker, defender));

        if (witnesses.Count > 0)
        {
            reports.Add(CreateWitnessProof(fightResult, attacker, defender, witnesses));
        }

        return reports;
    }

    private CombatReport CreateDefenderReport(FightResult fightResult, Player attacker, Player defender)
    {
        string message = fightResult.DefenderStatus switch
        {
            PlayerLifeStatus.Dead => DEATH_MESSAGE,
            PlayerLifeStatus.Stunned => $"The {attacker.Role} attacked you. You have been knocked out.",
            _ when fightResult.IsStoppedByHitAndRun => DescribeHitAndRunToDefender(fightResult, attacker),
            _ => $"The {attacker.Role} attacked you.",
        };

        return new CombatReport { Message = message, Recipients = [defender] };
    }

    private string DescribeHitAndRunToDefender(FightResult fightResult, Player attacker)
    {
        int identificationPercent = fightResult.WasDefenderSurprised
            ? CombatConstants.IDENTIFICATION_PERCENT_AFTER_SURPRISE
            : CombatConstants.IDENTIFICATION_PERCENT_WITHOUT_SURPRISE;

        bool isIdentified = _randomProvider.Next(0, 100) < identificationPercent;

        return isIdentified
            ? $"The {attacker.Role} targeted you with a hit-and-run."
            : "Someone targeted you with a hit-and-run. You could not see who.";
    }

    private static CombatReport CreateAttackerReport(FightResult fightResult, Player attacker, Player defender)
    {
        string outcome = fightResult.ExitType switch
        {
            FightExitType.StoppedByHitAndRun => "You withdrew after your hit-and-run.",
            FightExitType.BrokeOffUnderPushHard => "You broke off the attack.",
            FightExitType.BrokeOffWithoutStrength => "You had no strength left to fight and broke off.",
            _ => DescribeKnockoutToAttacker(fightResult),
        };

        return new CombatReport
        {
            Message = $"You attacked the {defender.Role}. {outcome}",
            Recipients = [attacker],
        };
    }

    private static string DescribeKnockoutToAttacker(FightResult fightResult)
    {
        if (fightResult.AttackerStatus == PlayerLifeStatus.Stunned)
        {
            return "You have been knocked out.";
        }

        return fightResult.DefenderStatus == PlayerLifeStatus.Dead
            ? "They died."
            : "They were knocked out.";
    }

    private static CombatReport CreateWitnessProof(FightResult fightResult, Player attacker, Player defender,
        List<Player> witnesses)
    {
        string message = $"You saw the {attacker.Role} attack the {defender.Role}. " +
                         $"{DescribeWeapons(attacker, fightResult.Attacker)} " +
                         $"{DescribeWeapons(defender, fightResult.Defender)}";

        return new CombatReport { Message = message, Recipients = witnesses };
    }

    private static string DescribeWeapons(Player player, Combatant combatant)
    {
        if (combatant.WeaponsUsed.Count == 0)
        {
            return $"The {player.Role} did not strike.";
        }

        string weapons = string.Join(" and ", combatant.WeaponsUsed.Select(DescribeWeapon));

        return $"The {player.Role} fought with {weapons}.";
    }

    private static string DescribeWeapon(Weapons weapon)
    {
        return weapon == Weapons.Unarmed ? "bare hands" : $"a {weapon.ToString().ToLowerInvariant()}";
    }
}
