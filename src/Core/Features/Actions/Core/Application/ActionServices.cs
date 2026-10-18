using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.Core.Application;

/// <summary>
/// Provides utility methods for discovering and executing all game actions
/// of a given base type.  
/// This service treats actions polymorphically: instead of resolving a single
/// action type, it scans the assembly for all concrete implementations of
/// action base classes and resolves them through dependency injection.
/// </summary>
public class ActionServices
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ActionCatalog _actionCatalog;

    public ActionServices(IServiceProvider serviceProvider, ActionCatalog actionCatalog)
    {
        _serviceProvider = serviceProvider;
        _actionCatalog = actionCatalog;
    }

    public async Task<List<InstantActionAvailability>> GetInstantActionAvailabilityResults(GetActionsRequest request)
    {
        IEnumerable<InstantActionBase> instantActionServices = GetInstantActions();

        // Get all the tasks of respective nature ( instants, with targets)  
        IEnumerable<InstantActionBase> instantActions = instantActionServices;

        List<InstantActionAvailability> instantActionAvailabilities = new();

        foreach (InstantActionBase instantActionAppService in instantActions)
        {
            InstantActionAvailability? availability = await instantActionAppService.GetAvailabilityResult(request);

            if (availability is not null)
            {
                instantActionAvailabilities.Add(availability);
            }
        }

        return instantActionAvailabilities;
    }

    public IEnumerable<InstantActionBase> GetInstantActions()
    {
        IEnumerable<InstantActionBase> services =
            _actionCatalog.InstantActionTypes.Select(t => (InstantActionBase)_serviceProvider.GetRequiredService(t));
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
            ActionWithTargetsAvailability? availability = await instantActionAppService.GetAvailabilityResult(request);

            if (availability is not null)
            {
                instantActionAvailabilities.Add(availability);
            }
        }

        return instantActionAvailabilities;
    }

    public IEnumerable<ActionWithTargetsBase> GetActionsWithTargets()
    {
        IEnumerable<ActionWithTargetsBase> services =
            _actionCatalog.ActionWithTargetsTypes.Select(t => (ActionWithTargetsBase)_serviceProvider.GetRequiredService(t));
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
        if (!_actionCatalog.ActionTypesByName.TryGetValue(actionName, out Type? type))
        {
            throw new InvalidOperationException($"No action named '{actionName}'.");
        }

        return (ActionBase)_serviceProvider.GetRequiredService(type);
    }
}