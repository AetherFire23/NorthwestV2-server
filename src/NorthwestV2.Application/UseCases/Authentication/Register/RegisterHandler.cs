using Mediator;

namespace NorthwestV2.Application.UseCases.Authentication.Register;

public class RegisterHandler : IRequestHandler<RegisterRequest>
{
    public async ValueTask<Unit> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}