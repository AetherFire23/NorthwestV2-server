using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations.PlayerInitializers;

public class MarineRoleInitializer : RoleInitializer
{
    public MarineRoleInitializer() : base(Roles.Marine)
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