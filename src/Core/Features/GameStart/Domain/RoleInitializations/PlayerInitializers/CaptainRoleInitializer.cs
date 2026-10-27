using NorthwestV2.Features.Actions.General.Combat.Items;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.GameStart.Domain.RoleInitializations.PlayerInitializers;

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
            Inventory = new Inventory { Items = [new Sword(), new Pistol()] }
        };

        return player;
    }
}