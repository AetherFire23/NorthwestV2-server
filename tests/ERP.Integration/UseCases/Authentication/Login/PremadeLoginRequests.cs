using NorthwestV2.Features.UseCases.Authentication.Login;

namespace NorthwestV2.Integration.UseCases.Authentication.Login;

public static class PremadeLoginRequests
{
    public static LoginRequest Fred = new LoginRequest()
    {
        Password = "123",
        Username = "Fred"
    };
}