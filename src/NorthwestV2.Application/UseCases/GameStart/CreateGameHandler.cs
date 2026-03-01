using Mediator;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.GameStart;

public class CreateGameHandler : IRequestHandler<CreateGameRequest, Guid>
{
    private readonly NorthwestContext _northwestContext;

    public CreateGameHandler(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public async ValueTask<Guid> Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        // Create Players
        

        return Guid.Empty;
    }
}