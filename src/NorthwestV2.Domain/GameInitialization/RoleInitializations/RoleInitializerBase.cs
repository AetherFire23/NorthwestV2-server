using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations;

public abstract class RoleInitializerBase
{
    public abstract Player CreateAndInitializePlayer(RoleInitializationContext context);
}