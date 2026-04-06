namespace NorthwestV2.Features.UseCases.Authentication.Login;

public class LoginResult
{
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
}