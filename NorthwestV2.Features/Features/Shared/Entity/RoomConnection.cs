namespace NorthwestV2.Features.Features.Shared.Entity;

public class RoomConnection
{
    public Guid Id { get; set; }

    public Guid Room1Id { get; set; }
    public required Room Room1 { get; set; }

    public Guid Room2Id { get; set; }
    public required Room Room2 { get; set; }
}