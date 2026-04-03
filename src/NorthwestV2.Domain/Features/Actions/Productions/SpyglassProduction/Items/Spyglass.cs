using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Items;

public class Spyglass : CommonItemBase
{
    public const int SpyglassStartCarryValue = 1;

    public Spyglass() : base(ItemTypes.Spyglass, SpyglassStartCarryValue)
    {
    }

    // TODO: Maybe enforce the room creation inside the Item ? 
}