using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class DoctorRoleInitializer : RoleInitializer
{
    public DoctorRoleInitializer() : base(Roles.Doctor)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        return null;
    }
}