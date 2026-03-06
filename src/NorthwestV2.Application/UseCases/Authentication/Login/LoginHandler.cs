using System.IdentityModel.Tokens.Jwt;
using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.UseCases.Authentication.Login.Tokens;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Practical;

namespace NorthwestV2.Application.UseCases.Authentication.Login;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResult>
{
    private readonly NorthwestContext _northwestContext;

    public LoginHandler(NorthwestContext northwestContext)
    {
        _northwestContext = northwestContext;
    }

    public async ValueTask<LoginResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        User user = await _northwestContext.Users.FirstAsync(u => u.Username == request.Username);

        // TODO: Hash and check passwords for real.

        if (user.HashedPassword != request.Password)
        {
            throw new LoginException();
        }


        return new LoginResult
        {
            UserId = user.Id,
        };
    }
}