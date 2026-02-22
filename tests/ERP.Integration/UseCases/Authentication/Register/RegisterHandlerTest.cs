using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Register;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Authentication.Register;

[TestSubject(typeof(RegisterHandler))]
public class RegisterHandlerTest : ErpIntegrationTestBase
{
    public RegisterHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenRegisterRequest_WhenRegister_ThenUserExisters()
    {
        RegisterRequest registerRequest = new RegisterRequest()
        {
            Username = "Fred",
            Password = "Password",
        };

        Mediator.Send(registerRequest);

        Assert.NotEmpty(Context.Users);
    }
}