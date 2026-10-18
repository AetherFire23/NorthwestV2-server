namespace NorthwestV2.UseCases.Authentication.Login;

public class LoginException : Exception
{
    public LoginException() : base("No matching Credentials.")
    {
    }
}
