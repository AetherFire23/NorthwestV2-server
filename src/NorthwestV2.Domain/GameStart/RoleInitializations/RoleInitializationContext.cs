using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.GameStart.RoleInitializations;

public class RoleInitializationContext
{
    public Game Game { get; set; }
    public List<Room> Rooms { get; set; }
    public User User { get; set; }
}