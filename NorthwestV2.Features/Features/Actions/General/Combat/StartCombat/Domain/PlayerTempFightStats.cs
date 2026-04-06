using NorthwestV2.Features.Features.Actions.Domain.Core;

namespace NorthwestV2.Features.Features.Actions.Domain.General.Combat;

public class PlayerTempFightStats
{
    public int Strength { get; set; }
    public int BaseToughness { get; set; }
    public int CurrentToughness { get; set; }
    public int CurrentHealth { get; set; }
    public AttackerStances AttackerStance { get; set; }


    public PlayerTempFightStats(int strength, int currentToughness, int currentHealth, int baseToughness,
        AttackerStances attackerStance)
    {
        Strength = strength;
        CurrentToughness = currentToughness;
        CurrentHealth = currentHealth;
        BaseToughness = baseToughness;
        AttackerStance = attackerStance;
    }
}