using System.Text.RegularExpressions;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthwestV2.Compose;
using NorthwestV2.Infrastructure;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace NorthwestV2.Integration;

public class NorthwestIntegrationTestBase //: PostgresTestContainer
{
    private readonly ITestOutputHelper _output;

    // TODO: Remove these two as they are not scoped. 
    protected readonly IMediator Mediator;
    protected readonly NorthwestContext Context;
    public readonly ServiceProvider _serviceProvider;
    public readonly IServiceScope _scope;

    public NorthwestIntegrationTestBase(ITestOutputHelper output)
    {
        _output = output;
        var instance =
            new PostgreSqlBuilder("postgres:18")
                .WithReuse(false)
                .Build();

        instance.StartAsync().Wait();

        Dictionary<string, string?> configurationDictionary = new();

        var cs = Regex.Replace(instance.GetConnectionString(), "Database=[a-z0-9]*;", $"Database=northwest;");


        configurationDictionary.Add("northwestConnectionString", cs);
        // TODO: add northwestConfig
        IConfigurationRoot configuration =
            new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();


        ServiceCollection serviceCollection = new();
        // AppComposer.Initialize(_serviceProvider);
        /*
         * Logging is outputted to every log outputter. ILoggerProvider.
         */
        serviceCollection.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new XUnitLoggerProvider(_output));
            builder.SetMinimumLevel(LogLevel.Information);
        });
        AppComposer.ComposeApplication(serviceCollection, configuration);

        _serviceProvider = serviceCollection.BuildServiceProvider();
        
        AppComposer.Initialize(_serviceProvider);


        _scope = _serviceProvider.CreateScope();
        Mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        Context = _scope.ServiceProvider.GetRequiredService<NorthwestContext>();
    }
}