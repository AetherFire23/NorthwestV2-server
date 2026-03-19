using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction.Stages;

public class SpyglassFirstStage : Stage
{
    public SpyglassFirstStage() : base(RoomEnum.Workshop, [ItemTypes.UnfinishedSpyglass])
    {
    }
}