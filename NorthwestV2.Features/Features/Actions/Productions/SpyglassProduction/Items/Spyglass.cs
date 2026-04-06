using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;

public class Spyglass : CommonItemBase
{
    public const int SpyglassStartCarryValue = 1;

    public Spyglass() : base(ItemTypes.Spyglass, SpyglassStartCarryValue)
    {
    }

    // TODO: Maybe enforce the room creation inside the Item ? 
}