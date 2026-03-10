using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class BruteRoleInitializer : RoleInitializer
{
    public BruteRoleInitializer() : base(Roles.Brute)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        return new Player()
        {
            Game = context.Game,
            Role = this.Role,
            Room = context.Rooms.First(),
            User = context.User,
            Toughness = ToughnessInitializationConstants.TOUGH,
        };
    }
}