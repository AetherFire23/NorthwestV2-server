namespace NorthwestV2.Features.Features.Shared.Entity;

/*
 * Self-referencing many relationships in ef core is the worst pain in the ass of my entire life.
 * I need to do this in order for everything to work.
 */
public class RoomConnection
{
    public Guid Id { get; set; }

    public Guid Room1Id { get; set; }

    public Guid Room2Id { get; set; }
}