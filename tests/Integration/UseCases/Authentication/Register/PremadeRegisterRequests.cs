using NorthwestV2.Features.UseCases.Authentication.Register;

namespace NorthwestV2.Integration.UseCases.Authentication.Register;

public static class PremadeRegisterRequests
{
    public static RegisterRequest Fred = new RegisterRequest()
    {
        Password = "123",
        Username = "Fred"
    };

    public static RegisterRequest Otello = new RegisterRequest()
    {
        Password = "123",
        Username = nameof(Otello)
    };
}