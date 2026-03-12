using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameStart.RoleInitializations.PlayerInitializers;

public class CaptainRoleInitializer : RoleInitializer
{
    public CaptainRoleInitializer() : base(Roles.Captain)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        Player player = new Player()
        {
            Game = context.Game,
            User = context.User,
            Room = context.Rooms.First(),
            Role = Roles.Captain,
            BaseToughness = ToughnessInitializationConstants.NORMAL,
        };

        return player;
    }
}