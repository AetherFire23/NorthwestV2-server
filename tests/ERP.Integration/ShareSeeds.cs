using AetherFire23.ERP.Domain;
using Mediator;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Application.UseCases.GameStart;
using NorthwestV2.Infrastructure;

namespace NorthwestV2.Integration;

public class ShareSeeds
{
    public static async Task<GameDataSeed> ArrangeUntilGameCreation(IMediator mediator, NorthwestContext context)
    {
        List<Guid> ids = new List<Guid>();

        for (int i = 0; i < GameSettings.RequiredPlayerCountToStartGame; i++)
        {
            Guid userId = await mediator.Send(new RegisterRequest()
            {
                Username = $"User{i}",
                Password = "123"
            });

            ids.Add(userId);
        }

        Guid gameId = await mediator.Send(new CreateGameRequest()
        {
            UserIds = ids
        });


        List<Guid> playerIds = context.Players
            .Where(x => x.GameId == gameId)
            .Select(x => x.Id)
            .ToList();

        GameDataSeed gameDataSeed = new GameDataSeed()
        {
            UserIds = ids,
            GameId = gameId,
            PlayerIds = playerIds
        };

        return gameDataSeed;
    }
}

public class GameDataSeed
{
    public required Guid GameId { get; set; }
    public required List<Guid> UserIds { get; set; }
    public required List<Guid> PlayerIds { get; set; }
}