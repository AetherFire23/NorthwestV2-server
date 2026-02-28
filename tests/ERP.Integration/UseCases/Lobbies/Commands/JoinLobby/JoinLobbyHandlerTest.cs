using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Lobbies.Commands;
using NorthwestV2.Application.UseCases.Lobbies.Commands.JoinLobby;
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

        await Mediator.Send(new JoinLobbyRequest()
        {
            UserId = lobbyJoinerId,
            LobbyId = lobbyId,
        });

        await base.Context.FindAsync(lobbyJoinerId);
        Assert.Equal(lobbyId,);
    }


    // TODO: verify lobbb

    // private SeedInfo CreateLobby()
    // {
    //     return new SeedInfo
    //     {
    //         LobbyCreatorId = 
    //     };
    // }


    private class SeedInfo
    {
        public required Guid LobbyId { get; set; }
        public required Guid LobbyCreatorId { get; set; }
    }
}