using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;

namespace AetherFire23.ERP.Domain.Entity;

public class Room : EntityBase
{
    public Guid GameId { get; set; }
    public required Game Game { get; set; }
    public required RoomEnum RoomEnum { get; set; }

    public List<Room> AdjacentRooms { get; set; } = [];

    public Inventory Inventory { get; set; } = new Inventory();


    public override string ToString()
    {
        return this.RoomEnum.ToString();
    }

    public ActionTarget ToTarget()
    {
        var actionTarget = new ActionTarget()
        {
            Name = this.RoomEnum.ToString(),
            TargetId = this.Id,
        };

        return actionTarget;
    }
}