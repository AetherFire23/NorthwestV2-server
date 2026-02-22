using Mediator;

namespace NorthwestV2.Application.Features.Commands.GameInitialization;

public class InitializeGameRequest : IRequest
{
    public IEnumerable<Guid> UserIds { get; set; } = [];
}