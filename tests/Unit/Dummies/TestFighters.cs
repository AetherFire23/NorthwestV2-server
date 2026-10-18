using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Unit.Dummies;

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
