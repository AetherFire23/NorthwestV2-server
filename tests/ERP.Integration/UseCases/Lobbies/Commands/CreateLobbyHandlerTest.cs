using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Authentication.Login;
using NorthwestV2.Features.UseCases.MainMenu.Lobbies.Commands.CreateLobby;
using NorthwestV2.Integration.Scratches;
using NorthwestV2.Integration.UseCases.Authentication.Login;
using NorthwestV2.Integration.UseCases.Authentication.Register;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Lobbies.Commands;

[TestSubject(typeof(CreateLobbyHandler))]
public class CreateLobbyHandlerTest : TestBase2
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