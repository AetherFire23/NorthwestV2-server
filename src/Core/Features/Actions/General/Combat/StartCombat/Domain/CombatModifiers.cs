using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Strength modifiers fixed at the start of a combat, in percent of the basic damage (docs/combat.md, Strength).
/// </summary>
public static class CombatModifiers
{
    /// <summary>
    /// Melee only.
    /// </summary>
    public static int HealthPenaltyPercent(int health)
    {
        return health switch
        {
            <= 20 => -20,
            <= 40 => -15,
            <= 60 => -10,
            <= 80 => -5,
            _ => 0,
        };
    }

    public static int HopePenaltyPercent(int hope)
    {
        return hope switch
        {
            <= 20 => -30,
            <= 40 => -10,
            _ => 0,
        };
    }

    /// <summary>
    /// Loyals suffer their real Hope penalty. Acolytes never do, and apply the penalty they chose to fake instead.
    /// </summary>
    public static int EffectiveHopePenaltyPercent(Player player, FakedHopePenalties fakedPenalty)
    {
        if (!player.IsAcolyte)
        {
            return HopePenaltyPercent(player.Hope);
        }

        return fakedPenalty switch
        {
            FakedHopePenalties.MatchTrackedHope => HopePenaltyPercent(player.Hope),
            FakedHopePenalties.None => 0,
            FakedHopePenalties.Minus10 => -10,
            FakedHopePenalties.Minus30 => -30,
            _ => throw new ArgumentOutOfRangeException(nameof(fakedPenalty), fakedPenalty, null),
        };
    }

    /// <summary>
    /// Only the attacker has a stance, and it only counts during the rounds.
    /// </summary>
    public static int StancePercent(AttackerStances stance)
    {
        return stance switch
        {
            AttackerStances.ToTheEnd => CombatConstants.TO_THE_END_PERCENT,
            AttackerStances.HitAndRun => CombatConstants.HIT_AND_RUN_PERCENT,
            AttackerStances.PushHard => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(stance), stance, null),
        };
    }
}
