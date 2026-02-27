using Mediator;

namespace NorthwestV2.Application.UseCases.Authentication.Register;

public class RegisterRequest : IRequest<Guid>
{
    public required string Username { get; set; }
    public required string Password { get; set; }

    public void Deconstruct(out string username, out string password)
    {
        username = this.Username;
        password = this.Password;
    }
}