using Mediator;

namespace NorthwestV2.UseCases.Items.Commands;

public class SwapItemOwnershipRequest : IRequest
{
    public required Guid PlayerId { get; set; }
    public required Guid ItemId { get; set; }
}