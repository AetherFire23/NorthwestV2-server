using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.Items;

/// <summary>
/// Strong ranged weapon, stronger outdoors. Not concealable. See docs/combat.md for its Strength.
/// </summary>
public class Musket : CommonItemBase
{
    public Musket() : base(ItemTypes.Musket, 3)
    {
    }
}
