using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.UseCases.Items.Queries;

public class GetAvailableItemsResponse
{
    public List<ItemDto> PlayerItems { get; set; } = [];
    public List<ItemDto> RoomItems { get; set; } = [];
}