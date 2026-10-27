using Mediator;

namespace NorthwestV2.UseCases.Combat.Queries;

public class GetMyCombatReactionsRequest : IRequest<GetMyCombatReactionsResponse>
{
    public required Guid PlayerId { get; set; }
}
