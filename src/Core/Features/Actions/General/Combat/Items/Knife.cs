using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.Items;

/// <summary>
/// Weak melee weapon. Concealable. See docs/combat.md for its Strength.
/// </summary>
public class Knife : CommonItemBase
{
    public Knife() : base(ItemTypes.Knife, 1)
    {
    }
}
