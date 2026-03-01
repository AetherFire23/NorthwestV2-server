using Mediator;

namespace NorthwestV2.Application.UseCases.GameStart;

/// <summary>
/// Returns the created Game Id 
/// </summary>
public class CreateGameRequest : IRequest<Guid>
{
    public IEnumerable<Guid> UserIds { get; set; } = [];
}