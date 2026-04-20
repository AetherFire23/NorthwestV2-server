using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AetherFire23.Commons.Composition;

// public static class Composer
// {
//     private static List<IInitializer> _initializers = [];
//
//     public static void InstallServices(this IServiceCollection serviceCollection, IConfiguration configuration,
//         params Assembly[] assemblies)
//     {
//         // Gets and activates the IInstallers in the assemblies. 
//         var installers = ScanInstallers(assemblies);
//
//         _initializers = installers.OfType<IInitializer>().ToList();
//
//         // Run the installers
//         foreach (IInstaller installer in installers)
//         {
//             installer.Install(serviceCollection, configuration);
//         }
//     }
//
//     public static void InitializeServices(this IServiceProvider serviceProvider)
//     {
//         // TODO: capture the registered services and un-register them. 
//         foreach (var initializer in _initializers)
//         {
//             initializer.Initialize(serviceProvider);
//         }
//     }
//
//     /*
//      * Some installers may have been deleted for testing / configuration.
//      * serviceCollection is a collection after all and it exposes .Remove() methods.
//      * We cache the _initializables to avoid multiple reflection calls, but we still have to ensure
//      */
//     public static void RemoveInstaller<TInitializer>(this IServiceCollection serviceCollection)
//         where TInitializer : IInitializer
//     {
//         _initializers.Remove(_initializers.First(x => x.GetType() == typeof(TInitializer)));
//     }
//
//     private static IEnumerable<IInstaller> ScanInstallers(IEnumerable<Assembly> assemblies)
//     {
//         var installers = assemblies
//             .SelectMany(x => x.GetTypes())
//             .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
//             .Select(x => (IInstaller)Activator.CreateInstance(x) ?? throw new InvalidOperationException("sdsd"));
//
//         return installers;
//     }
// }
//
// public static class Composer
// {
//     public static void InstallServices(this IServiceCollection serviceCollection, IConfiguration configuration,
//         params Assembly[] assemblies)
//     {
//         // Gets and activates the IInstallers in the assemblies. 
//         var installers = ScanInstallers(assemblies);
//
//         var initializers = installers.OfType<IInitializer>().ToList();
//
//         // Run the installers
//         foreach (IInstaller installer in installers)
//         {
//             installer.Install(serviceCollection, configuration);
//         }
//
//         serviceCollection.AddSingleton<InjectedComposer>(x => new InjectedComposer(initializers));
//     }
//
//     public static void InitializeServices(this IServiceProvider serviceProvider)
//     {
//         InjectedComposer composer = serviceProvider.GetRequiredService<InjectedComposer>();
//
//         // TODO: capture the registered services and un-register them. 
//         foreach (var initializer in composer.Initializers)
//         {
//             initializer.Initialize(serviceProvider);
//         }
//     }
//
//     // /*
//     //  * Some installers may have been deleted for testing / configuration.
//     //  * serviceCollection is a collection after all and it exposes .Remove() methods.
//     //  * We cache the _initializables to avoid multiple reflection calls, but we still have to ensure
//     //  */
//     // public static void RemoveInstaller<TInitializer>(this IServiceCollection serviceCollection)
//     //     where TInitializer : IInitializer
//     // {
//     //     _initializers.Remove(_initializers.First(x => x.GetType() == typeof(TInitializer)));
//     // }
//
//     private static IEnumerable<IInstaller> ScanInstallers(IEnumerable<Assembly> assemblies)
//     {
//         var installers = assemblies
//             .SelectMany(x => x.GetTypes())
//             .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
//             .Select(x => (IInstaller)Activator.CreateInstance(x) ?? throw new InvalidOperationException("sdsd"));
//
//         return installers;
//     }
// }
//
// public class InjectedComposer
// {
//     public List<IInitializer> Initializers { get; private set; }
//
//     public InjectedComposer(List<IInitializer> initializers)
//     {
//         Initializers = initializers;
//     }
// }

public sealed class Composer
{
    private readonly List<IInitializer> _initializers = [];

    public void InstallServices(
        IServiceCollection serviceCollection,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // Discover installers
        var installers = ScanInstallers(assemblies).ToList();

        // Cache initializers for later Initialize phase

        _initializers.AddRange(installers.OfType<IInitializer>());

        // Run installers
        foreach (var installer in installers)
        {
            installer.Install(serviceCollection, configuration);
        }
    }

    public void InitializeServices(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        
        
        foreach (IInitializer initializer in _initializers)
        {
            initializer.Initialize(scope.ServiceProvider);
        }
        
    }

    /*
     * Allows tests or configuration to disable a specific initializer
     * before InitializeServices is called.
     */
    public void RemoveInstaller<TInitializer>()
        where TInitializer : IInitializer
    {
        _initializers.RemoveAll(x => x is TInitializer);
    }

    /// <summary>
    /// Looks for all installers of type IInstaller in the assemblies provided in the parameters.
    /// It is trying to instantiate the installer 
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns>The instantiated sequences   </returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static IEnumerable<IInstaller> ScanInstallers(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IInstaller).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !t.IsAbstract)
            .Select(t =>
                (IInstaller)Activator.CreateInstance(t)
                ?? throw new InvalidOperationException($"Could not create {t.FullName}"));
    }
}