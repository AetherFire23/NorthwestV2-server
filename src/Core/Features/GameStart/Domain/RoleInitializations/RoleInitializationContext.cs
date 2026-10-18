using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.GameStart.Domain.RoleInitializations;

public class RoleInitializationContext
{
    public Game Game { get; set; }
    public List<Room> Rooms { get; set; }
    public User User { get; set; }
}