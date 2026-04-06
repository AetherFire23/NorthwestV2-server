using AetherFire23.Commons.Seeding;
using Mediator;
using NorthwestV2.Features.UseCases.Authentication.Register;
using NorthwestV2.Features.UseCases.GameStart;

namespace NorthwestV2.Seed;

public class SeededCompany : ISeeder
{
    private readonly IMediator _mediator;

    public SeededCompany(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SetupSeeding()
    {
        List<Guid> userids = new List<Guid>();
        for (int i = 0; i < 12; i++)
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
        // TODO: some seeding 
    }
}