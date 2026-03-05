using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ActionBases.Bases;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsHandler : IRequestHandler<GetActionsRequest, GetActionsResult>
{
    private readonly IServiceProvider _serviceProvider;

    public GetActionsHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<GetActionsResult> Handle(GetActionsRequest request, CancellationToken cancellationToken)
    {
        // Get all the tasks of respective nature ( instants, with targets)  
        var instantActions = GetGameActions();

        return new GetActionsResult
        {
            ActionWithTargets = new List<ActionWithTargetsAvailability>(),
            InstantActions = new List<InstantGameActionAvailability>(),
        };
    }

    private IEnumerable<InstantActionAppService> GetGameActions()
    {
        var types = typeof(GetActionsHandler).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(InstantActionAppService).IsAssignableFrom(x)).ToList();

        var services = types.Select(t => (InstantActionAppService)_serviceProvider.GetRequiredService(t));

        return services;
    }
}