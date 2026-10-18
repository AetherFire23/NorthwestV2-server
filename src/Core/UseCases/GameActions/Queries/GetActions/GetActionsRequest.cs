using Mediator;

namespace NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

public class GetActionsRequest : IRequest<GetActionsResult>
{
    public required Guid PlayerId { get; set; }
}