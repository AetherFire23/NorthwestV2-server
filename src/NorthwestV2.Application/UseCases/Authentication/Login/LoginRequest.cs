using Mediator;

namespace NorthwestV2.Application.UseCases.Authentication.Login;

public class LoginRequest : IRequest<LoginResult>
{
    public required string Password { get; set; }
    public required string Username { get; set; }
}