using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Infrastructure;
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
        
        base._scope.Dispose();

        var users = base.RootServiceProvider.CreateScope().ServiceProvider.GetRequiredService<NorthwestContext>();
        Assert.NotEmpty(users.Users);
    }
}