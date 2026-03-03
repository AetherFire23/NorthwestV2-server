using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Entity;
using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameStart;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.UseCases.GameStart;

[TestSubject(typeof(CreateGameHandler))]
public class CreateGameHandlerTest : NorthwestIntegrationTestBase
{
    public CreateGameHandlerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenGamePrerequisitesMet_WhenCreated_AllPlayersExist()
    {
        CreateGameSeedData seedData = await ArrangeUntilGameCreation();

        Guid gameId = await Mediator.Send(new CreateGameRequest
        {
            UserIds = seedData.UserIds
        });

        Game? game = await Context.Games.FindAsync(gameId);
        
        Assert.NotNull(game);
    }
    
    //TODO: 
    // Many more asserts relating to the creation of a game 
    
    // TODO ASSert that game exists in database
    // TODO Asser that crewsnest and mAINdECK are connected inside of the database 

    /// <summary>
    /// Arranges the database until we can create a game.
    /// The main idea is that the InitializeGame()
    /// actually doesn't depend on the JoinLobby. it's JoinLobby that calls CreateGame.
    /// So We need to code CreateGameHandler() before JoinLobby 
    /// It's 
    /// </summary>
    private async Task<CreateGameSeedData> ArrangeUntilGameCreation()
    {
        // Preconditions: 
        // have 12 users
        List<Guid> ids = new List<Guid>();

        for (int i = 0; i < GameSettings.RequiredPlayerCountToStartGame; i++)
        {
            Guid userId = await Mediator.Send(new RegisterRequest()
            {
                Username = $"User{i}",
                Password = "123"
            });

            ids.Add(userId);
        }

        return new CreateGameSeedData()
        {
            UserIds = ids
        };
    }

    private class CreateGameSeedData
    {
        public required IEnumerable<Guid> UserIds { get; set; }
    }
}