using Testcontainers.PostgreSql;

namespace NorthwestV2.Integration.Scratches;

public static class SharedContainerFixture
{
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