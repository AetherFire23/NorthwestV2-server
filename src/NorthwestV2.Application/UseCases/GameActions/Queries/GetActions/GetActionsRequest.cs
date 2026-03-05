using Mediator;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsRequest : IRequest<GetActionsResult>
{
    public required Guid PlayerId { get; set; }
}