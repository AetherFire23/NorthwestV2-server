using Mediator;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features;
using NorthwestV2.Features.Actions.Cooking.Items;
using NorthwestV2.Features.Actions.Productions.SpyglassProduction.Items;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.Authentication.Register;
using NorthwestV2.UseCases.GameStart;

namespace NorthwestV2.Seeding;

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
            player.GameLogs.Add(new GameLog()
            {
                Message = "sample log only for you :)"
            });

            player.ActionPoints += 9999;

            player.Inventory.Items.Add(new Scrap());
            player.Inventory.Items.Add(new Can());
            player.Inventory.Items.Add(new Can());
            player.Room = _context.Rooms.First(x => x.RoomEnum == RoomEnum.Workshop);
            player.Room.Inventory.Add(new Scrap());
        }

        await _context.SaveChangesAsync();

        // TODO: some seeding 
    }
}