using AetherFire23.ERP.Domain.Features.Actions.Productions.Core.Entities;

namespace AetherFire23.ERP.Domain.Entity;

/// <summary>
/// Normal components may be components for productions. 
/// </summary>
public class CommonItemBase : ItemBase
{
    /// <summary>
    /// A normal item is not always a component for a production. 
    /// </summary>
    public Guid? ProductionItemId { get; set; }

    public ProductionItemBase? ProductionItem { get; set; }
    public int TimePointsContributions { get; set; } = 0;
    public bool IsLocked { get; set; } = false;

    public CommonItemBase(ItemTypes itemType, int carryValue) : base(itemType, carryValue)
    {
    }

    public override string ToString()
    {
        return this.ItemType.ToString();
    }
}