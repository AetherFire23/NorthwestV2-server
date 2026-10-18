using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.UseCases.Authentication.Register;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NorthwestV2.Integration.UseCases.Authentication.Register;

[TestSubject(typeof(RegisterHandler))]
public class RegisterHandlerTest : NorthwestTestBase
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
        
        base.Scope.Dispose();

        var users = base.RootServiceProvider.CreateScope().ServiceProvider.GetRequiredService<NorthwestContext>();
        Assert.NotEmpty(users.Users);
    }
}