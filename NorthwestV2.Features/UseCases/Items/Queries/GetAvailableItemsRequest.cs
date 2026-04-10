using Mediator;

namespace NorthwestV2.Features.UseCases.Items.Queries;

public class GetAvailableItemsRequest : IRequest, IRequest<GetAvailableItemsResponse>
{
    public required Guid PlayerId { get; set; }
}