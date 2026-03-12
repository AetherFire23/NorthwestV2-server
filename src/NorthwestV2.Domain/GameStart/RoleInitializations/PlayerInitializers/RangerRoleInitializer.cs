using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameStart.RoleInitializations.PlayerInitializers;

public class RangerRoleInitializer : RoleInitializer
{
    public RangerRoleInitializer() : base(Roles.Ranger)
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
            BaseToughness = ToughnessInitializationConstants.NORMAL,
        };
    }
}