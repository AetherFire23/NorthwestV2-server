using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Lobbies.Commands.JoinLobby;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.Lobbies.Commands.JoinLobby;

[TestSubject(typeof(JoinLobbyHandler))]
public class JoinLobbyHandlerTest : NorthwestIntegrationTestBase
{
    public JoinLobbyHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GivenLobbyOtherPlayerCanJoinLobby()
    {
        
    }
}