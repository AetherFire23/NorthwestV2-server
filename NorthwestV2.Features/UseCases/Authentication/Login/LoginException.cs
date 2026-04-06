namespace NorthwestV2.Features.UseCases.Authentication.Login;

public class LoginException : Exception
{
    public LoginException() : base("No matching Credentials.")
    {
    }
}
