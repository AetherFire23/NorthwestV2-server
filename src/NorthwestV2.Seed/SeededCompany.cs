using AetherFire23.Commons.Seeding;
using Mediator;

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
     // TODO: some seeding 
    }
}