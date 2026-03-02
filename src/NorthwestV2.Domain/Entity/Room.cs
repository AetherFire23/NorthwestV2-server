using AetherFire23.Commons.Domain.Entities;

namespace AetherFire23.ERP.Domain.Entity;

public class Room : EntityBase
{
    public Guid GameId { get; set; }
    public required Game Game { get; set; }
    public required RoomEnum RoomEnum { get; set; }

    public ICollection<Room> AdjacentRooms { get; set; } = [];
}