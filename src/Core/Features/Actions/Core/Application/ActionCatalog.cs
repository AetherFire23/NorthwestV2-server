using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;

namespace NorthwestV2.Features.Features.Actions.Core.Application;

/// <summary>
/// Startup-time registry of every concrete game action type.
/// The assembly is scanned once so requests never touch reflection;
/// the actions themselves stay Scoped and are resolved from DI on demand.
/// </summary>
public class ActionCatalog
{
    private IReadOnlyDictionary<string, Type>? _actionTypesByName;

    private ActionCatalog(IReadOnlyList<Type> instantActionTypes, IReadOnlyList<Type> actionWithTargetsTypes,
        IReadOnlyList<Type> allActionTypes)
    {
        InstantActionTypes = instantActionTypes;
        ActionWithTargetsTypes = actionWithTargetsTypes;
        AllActionTypes = allActionTypes;
    }

    public IReadOnlyList<Type> InstantActionTypes { get; }

    public IReadOnlyList<Type> ActionWithTargetsTypes { get; }

    public IReadOnlyList<Type> AllActionTypes { get; }

    /// <summary>
    /// ActionName to concrete type. Only available after <see cref="Initialize"/>.
    /// </summary>
    public IReadOnlyDictionary<string, Type> ActionTypesByName =>
        _actionTypesByName ?? throw new InvalidOperationException(
            $"{nameof(ActionCatalog)} is not initialized. Call {nameof(Initialize)} at startup.");

    public static ActionCatalog Scan(Assembly assembly)
    {
        List<Type> concreteTypes = assembly.GetTypes().Where(x => !x.IsAbstract).ToList();

        return new ActionCatalog(
            Concrete<InstantActionBase>(concreteTypes),
            Concrete<ActionWithTargetsBase>(concreteTypes),
            Concrete<ActionBase>(concreteTypes));
    }

    /// <summary>
    /// Resolves each action once to read its ActionName. ActionName is set in the constructor,
    /// so this is the only way to learn it. Also fails at boot, not on a request, when an action
    /// is missing from DI or two actions share a name.
    /// </summary>
    public void Initialize(IServiceProvider serviceProvider)
    {
        Dictionary<string, Type> byName = new();

        using IServiceScope scope = serviceProvider.CreateScope();

        foreach (Type type in AllActionTypes)
        {
            ActionBase action = (ActionBase)scope.ServiceProvider.GetRequiredService(type);

            if (!byName.TryAdd(action.ActionName, type))
            {
                throw new InvalidOperationException(
                    $"Duplicate action name '{action.ActionName}' on {byName[action.ActionName].Name} and {type.Name}.");
            }
        }

        _actionTypesByName = byName;
    }

    private static List<Type> Concrete<T>(List<Type> concreteTypes)
    {
        return concreteTypes.Where(x => typeof(T).IsAssignableFrom(x)).ToList();
    }
}
