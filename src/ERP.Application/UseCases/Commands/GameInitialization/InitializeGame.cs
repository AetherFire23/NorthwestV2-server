using Mediator;

namespace NorthwestV2.Application.Features.Commands.GameInitialization;

public class InitializeGame : IRequestHandler<InitializeGameRequest>
{
    public async ValueTask<Unit> Handle(InitializeGameRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        return Unit.Value;
    }
}