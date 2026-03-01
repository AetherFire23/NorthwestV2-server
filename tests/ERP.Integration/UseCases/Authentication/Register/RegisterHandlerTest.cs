using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Register;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Authentication.Register;

[TestSubject(typeof(RegisterHandler))]
public class RegisterHandlerTest : NorthwestIntegrationTestBase
{
    public RegisterHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenRegisterRequest_WhenHandleRegister_ThenUserExists()
    {
        RegisterRequest registerRequest = new RegisterRequest()
        {
            Username = "Fred",
            Password = "Password",
        };

        await Mediator.Send(registerRequest);

        Assert.NotEmpty(Context.Users);
    }
}