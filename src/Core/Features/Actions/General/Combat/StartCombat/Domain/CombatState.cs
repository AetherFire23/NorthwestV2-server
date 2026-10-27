using NorthwestV2.Features.Actions.Core.Domain;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// A combat in progress. Cloneable so that <see cref="CombatResolver"/> can play a branch ahead without touching the real one.
/// </summary>
public class CombatState
{
    public required Combatant Attacker { get; init; }
    public required Combatant Defender { get; init; }
    public required AttackerStances Stance { get; init; }
    public required bool IsDefenderSurprised { get; init; }

    public FightExitType? ExitType { get; set; }

    public bool IsOver => ExitType.HasValue;

    public static CombatState Start(CombatSetup setup)
    {
        CombatState state = new CombatState
        {
            Attacker = setup.Attacker.Clone(),
            Defender = setup.Defender.Clone(),
            Stance = setup.Stance,
            IsDefenderSurprised = setup.IsDefenderSurprised,
        };

        return state;
    }

    public CombatState Clone()
    {
        CombatState clone = new CombatState
        {
            Attacker = Attacker.Clone(),
            Defender = Defender.Clone(),
            Stance = Stance,
            IsDefenderSurprised = IsDefenderSurprised,
            ExitType = ExitType,
        };

        return clone;
    }

    public Combatant OpponentOf(Combatant combatant)
    {
        return ReferenceEquals(combatant, Attacker) ? Defender : Attacker;
    }
}
