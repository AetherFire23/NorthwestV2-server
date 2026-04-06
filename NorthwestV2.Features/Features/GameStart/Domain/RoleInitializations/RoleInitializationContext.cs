using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.RoleInitializations;

public class RoleInitializationContext
{
    public Game Game { get; set; }
    public List<Room> Rooms { get; set; }
    public User User { get; set; }
}