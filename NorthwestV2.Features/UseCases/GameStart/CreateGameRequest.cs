using Mediator;

namespace NorthwestV2.Features.UseCases.GameStart;

/// <summary>
/// Returns the created Game I'd 
/// </summary>
public class CreateGameRequest : IRequest<Guid>
{
    public required IEnumerable<Guid> UserIds { get; set; } = [];
}