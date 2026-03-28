using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Role;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

public abstract class Stage
{
    public RoomEnum RequiredRoom { get; set; }
    public List<ItemTypes> RequiredItems { get; set; }
    public ItemTypes CreatedItemType { get; set; }
    public Roles? SpecializedTp { get; set; } = null;

    public bool IsSpecializedTpRequirement => SpecializedTp != null;

    protected Stage(RoomEnum requiredRoom, List<ItemTypes> requiredItems)
    {
        RequiredRoom = requiredRoom;
        RequiredItems = requiredItems;
    }


  
}