using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameStart.RoleInitializations.PlayerInitializers;

public class SapperRoleInitializer : RoleInitializer
{
    public SapperRoleInitializer() : base(Roles.Sapper)
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
            BaseToughness = ToughnessInitializationConstants.TOUGH,
            Inventory = new Inventory()
        };
    }
}