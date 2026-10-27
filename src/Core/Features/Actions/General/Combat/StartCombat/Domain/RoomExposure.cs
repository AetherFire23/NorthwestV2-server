using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Whether a room counts as outside for the weapon table. Every room not listed is inside.
/// </summary>
public static class RoomExposure
{
    public static readonly IReadOnlyList<RoomEnum> OutsideRooms =
    [
        RoomEnum.CrowsNest,
        RoomEnum.MainDeck,
        RoomEnum.ForeCastle,
        RoomEnum.QuarterDeck,
    ];

    public static bool IsOutside(RoomEnum room)
    {
        return OutsideRooms.Contains(room);
    }
}
