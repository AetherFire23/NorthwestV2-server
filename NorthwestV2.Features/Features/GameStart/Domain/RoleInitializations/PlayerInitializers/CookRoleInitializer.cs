using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.RoleInitializations.PlayerInitializers;

public class CookRoleInitializer : RoleInitializer
{
    public CookRoleInitializer() : base(Roles.Cook)
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
            Inventory = new Inventory()
        };
    }
}