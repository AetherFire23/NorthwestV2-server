using Mediator;

namespace NorthwestV2.UseCases.Items.Queries;

public class GetAvailableItemsRequest : IRequest<GetAvailableItemsResponse>
{
    public required Guid PlayerId { get; set; }
}