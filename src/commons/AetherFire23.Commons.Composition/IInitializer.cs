namespace AetherFire23.Commons.Composition;

/// <summary>
/// Uses the service provider in order to initialize some services i.e. : database that could need seeding and / or Migration.
/// </summary>
public interface IInitializer : IInstaller
{
    public void Initialize(IServiceProvider serviceProvider);
}