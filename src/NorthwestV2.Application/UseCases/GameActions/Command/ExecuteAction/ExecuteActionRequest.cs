using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using Mediator;

namespace NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;

public class ExecuteActionRequest : IRequest
{
    /// <summary>
    /// Name of the action (use ActionNames.YourAction)
    /// </summary>
    public required string ActionName { get; set; }
    public required Guid PlayerId { get; set; }
    
    /// <summary>
    /// Represents a sequence of screens, of target selections. 
    /// </summary>
    public List<List<ActionTarget>> ActionTargets { get; set; } = [];
}