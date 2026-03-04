using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class Cook : RoleInitializer
{
    public Cook() : base(Roles.Cook)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        return null;
    }
}