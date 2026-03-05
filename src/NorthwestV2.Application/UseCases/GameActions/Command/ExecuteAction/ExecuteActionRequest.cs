using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using Mediator;

namespace NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;

public class ExecuteActionRequest : IRequest
{
    public required string ActionName { get; set; }
    public required Guid PlayerId { get; set; }
    public List<List<ActionTarget>> ActionTargets { get; set; } = [];
}