using Mediator;

namespace NorthwestV2.Application.UseCases.Authentication.Login;

public class LoginRequest : IRequest
{
    public string Password { get; set; } = string.Empty;
}