// using System.Text.RegularExpressions;
// using Mediator;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using NorthwestV2.Compose;
// using NorthwestV2.Infrastructure;
// using Testcontainers.PostgreSql;
// using Xunit.Abstractions;
//
// namespace NorthwestV2.Integration;
//
// public class NorthwestIntegrationTestBase //: PostgresTestContainer
// {
//     private readonly ITestOutputHelper _output;
//
//     // TODO: Remove these two as they are not scoped. 
//     protected IMediator Mediator => _scope.ServiceProvider.GetRequiredService<IMediator>();
//     protected NorthwestContext Context => _scope.ServiceProvider.GetRequiredService<NorthwestContext>();
//     public IServiceScope _scope;
//     protected IServiceProvider RootServiceProvider;
//  
//     public NorthwestIntegrationTestBase(ITestOutputHelper output)
//     {
//         _output = output;
//         PostgreSqlContainer? instance =
//             new PostgreSqlBuilder("postgres:18")
//                 .WithDatabase($"northwest-{Guid.NewGuid()}")
//                 .WithReuse(true)
//                 .Build();
//
//         instance.StartAsync().Wait();
//
//         Dictionary<string, string?> configurationDictionary = new();
//
//         configurationDictionary.Add("northwestConnectionString", instance.GetConnectionString());
//         // TODO: add northwestConfig
//         IConfigurationRoot configuration =
//             new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();
//
//
//         ServiceCollection serviceCollection = new();
//         // AppComposer.Initialize(_serviceProvider);
//         /*
//          * Logging is outputted to every log outputter. ILoggerProvider.
//          */
//         serviceCollection.AddLogging(builder =>
//         {
//             builder.ClearProviders();
//             builder.AddProvider(new XUnitLoggerProvider(_output));
//             builder.SetMinimumLevel(LogLevel.Information);
//         });
//         AppComposer.ComposeApplication(serviceCollection, configuration);
//
//
//         RootServiceProvider = serviceCollection.BuildServiceProvider();
//
//         AppComposer.Initialize(RootServiceProvider);
//
//         _scope = RootServiceProvider.CreateScope();
//     }
//
//     protected T GetServiceFromScope<T>() where T : notnull
//     {
//         var service = this._scope.ServiceProvider.GetRequiredService<T>();
//
//         return service;
//     }
// }