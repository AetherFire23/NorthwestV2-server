using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class Scholar : RoleInitializer
{
    public Scholar() : base(Roles.Scholar)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        return null;
    }
}