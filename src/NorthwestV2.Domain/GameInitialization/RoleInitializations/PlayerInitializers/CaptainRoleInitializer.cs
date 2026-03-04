using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.GameInitialization.RoleInitializations.PlayerInitializers;

public class CaptainRoleInitializer : RoleInitializer
{
    public CaptainRoleInitializer() : base(Roles.Captain)
    {
    }

    public override Player CreateAndInitializePlayer(RoleInitializationContext context)
    {
        var player = new Player()
        {
            Game = context.Game,
            User = context.User,
            Room = context.Rooms.First(),
            Role = Roles.Brute
        };

        return player;
    }
}