using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Dummies;

public static class TestFighters
{
    public static Player CreateFighter(int toughness, Roles role = Roles.Engineer)
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);
        Player fighter = TestPlayers.CreateTestPlayer(toughness, role, room, game);

        return fighter;
    }
}
