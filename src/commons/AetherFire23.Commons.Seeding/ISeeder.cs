namespace AetherFire23.Commons.Seeding;

/// <summary>
/// Is injected inside the service provider. 
/// 
/// </summary>
public interface ISeeder
{
    /// <summary>
    /// The actual seeding process 
    /// </summary>
    /// <returns></returns>
    public Task SetupSeeding();
}