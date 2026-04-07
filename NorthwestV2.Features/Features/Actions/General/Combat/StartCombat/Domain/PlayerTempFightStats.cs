using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public class PlayerTempFightStats
{
    public required Player Player { get; set; }
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