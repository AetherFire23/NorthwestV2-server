namespace NorthwestV2.Application.UseCases.Authentication.Login;

public class LoginException : Exception
{
    public LoginException() : base("No matching Credentials.")
    {
    }
}
