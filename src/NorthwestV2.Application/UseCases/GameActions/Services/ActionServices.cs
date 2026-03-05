using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.Application.UseCases.GameActions.Services.ActionBases.Bases;

namespace NorthwestV2.Application.UseCases.GameActions.Services;

/// <summary>
/// Utility methods to retrieve all the actions of a given base type all at once.
/// Treats all actions as a polymorphic mass 
/// </summary>
public class ActionServices
{
    // TODO: Optimize
    private readonly IServiceProvider _serviceProvider;

    public ActionServices(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<InstantActionAvailability>> GetInstantActionAvailabilityResults(GetActionsRequest request)
    {
        IEnumerable<InstantActionAppService> services = GetInstantActions();

        // Get all the tasks of respective nature ( instants, with targets)  
        IEnumerable<InstantActionAppService> instantActions = services;

        List<InstantActionAvailability> instantActionAvailabilities = new();

        foreach (InstantActionAppService instantActionAppService in instantActions)
        {
            InstantActionAvailability availability = await instantActionAppService.GetAvailabilityResult(request);
            instantActionAvailabilities.Add(availability);
        }

        return instantActionAvailabilities;
    }

    public IEnumerable<InstantActionAppService> GetInstantActions()
    {
        List<Type> types = typeof(GetActionsHandler).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(InstantActionAppService).IsAssignableFrom(x)).ToList();

        IEnumerable<InstantActionAppService> services =
            types.Select(t => (InstantActionAppService)_serviceProvider.GetRequiredService(t));
        return services;
    }

    public async Task<List<ActionWithTargetsAvailability>> GetActionWithTargetsAvailabilityResults(GetActionsRequest request)
    {
        IEnumerable<ActionWithTargetsAppService> services = GetActionsWithTargets();

        // Get all the tasks of respective nature ( instants, with targets)  
        IEnumerable<ActionWithTargetsAppService> instantActions = services;

        List<ActionWithTargetsAvailability> instantActionAvailabilities = new();

        foreach (ActionWithTargetsAppService instantActionAppService in instantActions)
        {
            ActionWithTargetsAvailability availability = await instantActionAppService.GetAvailabilityResult(request);
            instantActionAvailabilities.Add(availability);
        }

        return instantActionAvailabilities; 
    }

    public IEnumerable<ActionWithTargetsAppService> GetActionsWithTargets()
    {
        List<Type> types = typeof(GetActionsHandler).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(ActionWithTargetsAppService).IsAssignableFrom(x)).ToList();

        IEnumerable<ActionWithTargetsAppService> services =
            types.Select(t => (ActionWithTargetsAppService)_serviceProvider.GetRequiredService(t));
        return services;
    }
}