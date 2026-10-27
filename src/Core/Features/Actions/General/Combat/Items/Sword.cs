using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.Items;

/// <summary>
/// Strong melee weapon, stronger outdoors. Not concealable. See docs/combat.md for its Strength.
/// </summary>
public class Sword : CommonItemBase
{
    public Sword() : base(ItemTypes.Sword, 2)
    {
    }
}
