using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.Lobbies.Commands;
using NorthwestV2.Application.UseCases.Lobbies.Commands.CreateLobby;
using NorthwestV2.Integration.UseCases.Authentication.Login;
using NorthwestV2.Integration.UseCases.Authentication.Register;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Lobbies.Commands;

[TestSubject(typeof(CreateLobbyHandler))]
public class CreateLobbyHandlerTest : NorthwestIntegrationTestBase
{
    public CreateLobbyHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenCreateLobbyRequest_WhenRequested_ThenLobbyCreated()
    {
        Guid userId = await Mediator.Send(PremadeRegisterRequests.Fred);

        LoginResult loginResult = await Mediator.Send(PremadeLoginRequests.Fred);

        Guid lobbyId = await Mediator.Send(new CreateLobbyRequest()
        {
            UserId = loginResult.UserId
        });

        Assert.NotEqual(lobbyId, Guid.Empty);
    }
}