using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Authentication.Login;

[TestSubject(typeof(LoginHandler))]
public class LoginHandlerTest : NorthwestIntegrationTestBase
{
    public LoginHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    private async Task GivenRegistration_WhenLogin_ThenReturnsDto()
    {
        await SeedRegistration();

        LoginResult loginResult = await base.Mediator.Send(new LoginRequest
        {
            Username = "Fred",
            Password = "Password"
        });

        Assert.NotNull(loginResult.UserId);
    }
    

    private async Task<SeedResult> SeedRegistration()
    {
        RegisterRequest registerRequest = new RegisterRequest()
        {
            Username = "Fred",
            Password = "Password",
        };

        Guid userId = await base.Mediator.Send(registerRequest);

        return new SeedResult()
        {
            UserId = userId
        };
    }

    private class SeedResult
    {
        public required Guid UserId { get; set; }
    }
}