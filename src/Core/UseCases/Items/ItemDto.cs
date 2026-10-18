using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.Items;

public class ItemDto
{
    public required Guid Id { get; set; }

    /// <summary>
    /// The inventoryId 
    /// </summary>
    public required Guid InventoryId { get; set; }


    public required ItemTypes ItemTypes { get; set; }
}