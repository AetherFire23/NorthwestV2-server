using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.UseCases.MainMenu.Lobbies.Commands.CreateLobby;
using NorthwestV2.Application.UseCases.MainMenu.Lobbies.Commands.JoinLobby;
using NorthwestV2.Integration.UseCases.Authentication.Register;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Lobbies.Commands.JoinLobby;

[TestSubject(typeof(JoinLobbyHandler))]
public class JoinLobbyHandlerTest : NorthwestIntegrationTestBase
{
    public JoinLobbyHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenJoiningUser_JoinsLobby_ThenIsMarkedAsJoinedInsideJoiningUser()
    {
        Guid lobbyCreatorId = await Mediator.Send(PremadeRegisterRequests.Fred);
        Guid lobbyJoinerId = await Mediator.Send(PremadeRegisterRequests.Otello);
        Guid lobbyId = await Mediator.Send(new CreateLobbyRequest
        {
            UserId = lobbyCreatorId,
        });

        await Mediator.Send(new JoinLobbyRequest
        {
            UserId = lobbyJoinerId,
            LobbyId = lobbyId
        });

        User user = await base.Context.Users
            .Include(x => x.Lobby)
            .FirstAsync(u => u.Id == lobbyJoinerId) ?? throw new InvalidOperationException("Null");

        Assert.Equal(lobbyId, user?.Lobby?.Id);
    }

    [Fact]
    public async Task GivenLobby_WhenUserJoins_ThenHasPlayerInIt()
    {
        Guid lobbyCreatorId = await Mediator.Send(PremadeRegisterRequests.Fred);
        Guid lobbyJoinerId = await Mediator.Send(PremadeRegisterRequests.Otello);
        Guid lobbyId = await Mediator.Send(new CreateLobbyRequest
        {
            UserId = lobbyCreatorId,
        });

        await Mediator.Send(new JoinLobbyRequest
        {
            UserId = lobbyJoinerId,
            LobbyId = lobbyId
        });

        Lobby lobby = await Context.Lobbies
            .Include(l => l.Users)
            .FirstAsync(x => x.Id == lobbyId);

        Assert.NotEmpty(lobby.Users);
    }

    private class SeedInfo
    {
        public required Guid LobbyId { get; set; }
        public required Guid LobbyCreatorId { get; set; }
    }
}