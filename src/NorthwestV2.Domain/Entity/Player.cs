using AetherFire23.Commons.Domain.Entities;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Entity;

// TODO: Consider using a base class for this 
public class Player : EntityBase
{
    public Guid UserId { get; set; }

    public required User User { get; set; }

    public required Roles Role { get; set; }

    public Guid GameId { get; set; }
    public required Game Game { get; set; }

    public Guid RoomId { get; set; }
    public required Room Room { get; set; }

    public override string ToString()
    {
        return this.Role.ToString();
    }
}