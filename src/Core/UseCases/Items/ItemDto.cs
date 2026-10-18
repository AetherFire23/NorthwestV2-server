using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.UseCases.Items;

public class ItemDto
{
    public required Guid Id { get; set; }

    /// <summary>
    /// The inventoryId 
    /// </summary>
    public required Guid InventoryId { get; set; }


    public required ItemTypes ItemTypes { get; set; }
}