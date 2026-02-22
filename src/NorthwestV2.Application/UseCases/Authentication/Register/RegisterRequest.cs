using Mediator;

namespace NorthwestV2.Application.UseCases.Authentication.Register;

public class RegisterRequest : IRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}