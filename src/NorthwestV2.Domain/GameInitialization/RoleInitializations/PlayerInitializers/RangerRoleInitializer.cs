using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class RangerRoleInitializer : RoleInitializer
{
    public RangerRoleInitializer() : base(Roles.Ranger)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        return null;
    }
}