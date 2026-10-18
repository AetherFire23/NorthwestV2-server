using Testcontainers.PostgreSql;

namespace NorthwestV2.Integration.Tooling;

/// <summary>
/// Creates only 1 share container for all instances. 
/// </summary>
public class SharedContainerFixture
{
    /// <summary>
    /// I don't really need lazy; however I need Thread safety.
    /// And this tool allows me to get that almost for free
    /// How this wosk is some TestBase will call GetAsync, and will not have to care about whether the contianer is started
    /// or currently being accessed. This Lazy Prop will manage that for it. 
    /// </summary>
    private static readonly Lazy<Task<PostgreSqlContainer>> _container
        = new Lazy<Task<PostgreSqlContainer>>(StartAsync, LazyThreadSafetyMode.ExecutionAndPublication);

    public static Task<PostgreSqlContainer> GetAsync() => _container.Value;

    private static async Task<PostgreSqlContainer> StartAsync()
    {
        PostgreSqlContainer? container = new PostgreSqlBuilder("postgres:18")
            .WithDatabase("postgres")
            .WithReuse(true)
            .Build();

        await container.StartAsync();
        return container;
    }
}