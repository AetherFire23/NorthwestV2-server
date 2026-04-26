using AetherFire23.Commons.Seeding;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.Features;
using NorthwestV2.Features.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.Authentication.Register;
using NorthwestV2.Features.UseCases.GameStart;
using NorthwestV2.Infrastructure;

namespace NorthwestV2.Seed;

public class SeededCompany : ISeeder
{
    private readonly IMediator _mediator;
    private readonly NorthwestContext _context;

    public SeededCompany(IMediator mediator, NorthwestContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task SetupSeeding()
    {
        List<Guid> userids = new List<Guid>();
        for (int i = 0; i < GameSettings.RequiredPlayerCountToStartGame; i++)
        {
            Guid userId = await _mediator.Send(new RegisterRequest
            {
                Password = "123",
                Username = $"myuser-{i}"
            });

            userids.Add(userId);
        }

        await _mediator.Send(new CreateGameRequest()
        {
            UserIds = userids
        });

        foreach (Player player in _context.Players.Include(player => player.Inventory).Include(player => player.Room)
                     .ThenInclude(room => room.Inventory).ToList())
        {
            player.Logs.Add(new GameLog
            {
                Message = "sample log only for you :)",
            });

            player.Inventory.Items.Add(new Scrap());
            player.Room = _context.Rooms.First(x => x.RoomEnum == RoomEnum.Workshop);
            player.Room.Inventory.Add(new Scrap());
        }

        await _context.SaveChangesAsync();

        // TODO: some seeding 
    }
}