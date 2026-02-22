using Mediator;

namespace NorthwestV2.Application.UseCases.Authentication.Login;

public class LoginHandler : IRequestHandler<LoginRequest>
{
    public async ValueTask<Unit> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        
        return Unit.Value;
    }
}