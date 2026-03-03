using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations;

public abstract class RoleInitializer
{
    public abstract Player CreateAndInitializePlayer(RoleInitializationContext context);

    public Roles Role { get; }

    protected RoleInitializer(Roles role)
    {
        this.Role = role;
    }
}