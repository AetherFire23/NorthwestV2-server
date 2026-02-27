using Mediator;

namespace NorthwestV2.Application.UseCases.Commands.GameInitialization;

public class InitializeGameRequest : IRequest
{
    public IEnumerable<Guid> UserIds { get; set; } = [];
}