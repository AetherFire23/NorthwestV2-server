using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.Items;

/// <summary>
/// Weak ranged weapon. Concealable. See docs/combat.md for its Strength.
/// </summary>
public class Pistol : CommonItemBase
{
    public Pistol() : base(ItemTypes.Pistol, 2)
    {
    }
}
