using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.Integration.UseCases.Authentication.Login;
using NorthwestV2.Integration.UseCases.Authentication.Register;
using NorthwestV2.UseCases.Authentication.Login;
using NorthwestV2.UseCases.MainMenu.Lobbies.Commands.CreateLobby;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Lobbies.Commands;

[TestSubject(typeof(CreateLobbyHandler))]
public class CreateLobbyHandlerTest : NorthwestTestBase
{
    public CreateLobbyHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenUser_WhenCreateLobby_ThenLobbyIdNotNull()
    {
        Guid userId = await Mediator.Send(PremadeRegisterRequests.Fred);
        LoginResult loginResult = await Mediator.Send(PremadeLoginRequests.Fred);

        Guid lobbyId = await Mediator.Send(new CreateLobbyRequest()
        {
            UserId = loginResult.UserId
        });

        Assert.NotEqual(lobbyId, Guid.Empty);
    }

    [Fact]
    public async Task GivenUser_WhenCreateLobby_ThenUserJoinsLobby()
    {
        Guid lobbyCreatorId = await Mediator.Send(PremadeRegisterRequests.Fred);
        Guid lobbyId = await Mediator.Send(new CreateLobbyRequest
        {
            UserId = lobbyCreatorId,
        });
        
        User user = await Context.Users
            .Include(x=> x.Lobby)
            .FirstAsync(u => u.Id == lobbyCreatorId);

        Assert.NotNull(user.Lobby);
    }
}