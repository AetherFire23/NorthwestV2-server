using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class MarineRoleInitializer : RoleInitializer
{
    public MarineRoleInitializer() : base(Roles.Marine)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        return null;
    }
}