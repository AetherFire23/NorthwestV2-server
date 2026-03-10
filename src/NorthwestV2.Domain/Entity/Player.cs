using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using AetherFire23.Commons.Domain.Entities;
using AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Entity;

// TODO: Consider using a base class for this 
public class Player : EntityBase
{
    [NotMapped] [NonSerialized] public const int INITIALIZATION_HEALTH = 100;

    public Guid UserId { get; set; }

    public required User User { get; set; }

    public required Roles Role { get; set; }

    public Guid GameId { get; set; }
    public required Game Game { get; set; }

    public Guid RoomId { get; set; }
    public required Room Room { get; set; }

    public Guid InventoryId { get; set; }
    public Inventory Inventory { get; set; } = new Inventory(); // Empty inventory by default 

    public int ActionPoints { get; set; } = 8; // default 8

    public int Health { get; set; } = INITIALIZATION_HEALTH;

    /// <summary>
    /// How much strength must be accumulated before health is lost.
    /// Depends per-role.
    /// </summary>
    public required int Toughness { get; set; }

    public DefensiveCounters DefensiveCounter { get; set; } = DefensiveCounters.DefaultVsStun;

    public override string ToString()
    {
        return this.Role.ToString();
    }
}