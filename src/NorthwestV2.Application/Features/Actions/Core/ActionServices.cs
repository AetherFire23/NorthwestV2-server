using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Actions.Feature.Availability.Instant;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Core;

/// <summary>
/// Provides utility methods for discovering and executing all game actions
/// of a given base type.  
/// 
/// This service treats actions polymorphically: instead of resolving a single
/// action type, it scans the assembly for all concrete implementations of
/// action base classes and resolves them through dependency injection.
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
        IEnumerable<InstantActionBase> services = GetInstantActions();

        // Get all the tasks of respective nature ( instants, with targets)  
        IEnumerable<InstantActionBase> instantActions = services;

        List<InstantActionAvailability> instantActionAvailabilities = new();

        foreach (InstantActionBase instantActionAppService in instantActions)
        {
            InstantActionAvailability availability = await instantActionAppService.GetAvailabilityResult(request);
            instantActionAvailabilities.Add(availability);
        }

        return instantActionAvailabilities;
    }

    public IEnumerable<InstantActionBase> GetInstantActions()
    {
        List<Type> types = typeof(GetActionsHandler).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(InstantActionBase).IsAssignableFrom(x)).ToList();

        IEnumerable<InstantActionBase> services =
            types.Select(t => (InstantActionBase)_serviceProvider.GetRequiredService(t));
        return services;
    }

    public async Task<List<ActionWithTargetsAvailability>> GetActionWithTargetsAvailabilityResults(
        GetActionsRequest request)
    {
        IEnumerable<ActionWithTargetsBase> services = GetActionsWithTargets();

        // Get all the tasks of respective nature ( instants, with targets)  
        IEnumerable<ActionWithTargetsBase> instantActions = services;

        List<ActionWithTargetsAvailability> instantActionAvailabilities = new();

        foreach (ActionWithTargetsBase instantActionAppService in instantActions)
        {
            ActionWithTargetsAvailability availability = await instantActionAppService.GetAvailabilityResult(request);
            instantActionAvailabilities.Add(availability);
        }

        return instantActionAvailabilities;
    }

    public IEnumerable<ActionWithTargetsBase> GetActionsWithTargets()
    {
        List<Type> types = typeof(GetActionsHandler).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(ActionWithTargetsBase).IsAssignableFrom(x)).ToList();

        IEnumerable<ActionWithTargetsBase> services =
            types.Select(t => (ActionWithTargetsBase)_serviceProvider.GetRequiredService(t));
        return services;
    }

    /// <summary>
    /// Scans all the actions for one given as a parameter.
    /// Typically comes from ExecuteActionRequest
    /// </summary>
    /// <param name="actionName"></param>
    /// <returns></returns>
    public async Task<ActionBase> GetActionFromName(string actionName)
    {
        List<Type> types = typeof(GetActionsHandler).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(ActionBase).IsAssignableFrom(x)).ToList();

        IEnumerable<ActionBase> bases = types.Select(x => (ActionBase)_serviceProvider.GetRequiredService(x));

        ActionBase actionBase = bases.First(x => x.ActionName == actionName);

        return actionBase;
    }
}