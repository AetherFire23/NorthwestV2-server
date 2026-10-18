using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Api.Composition;
using NorthwestV2.Infrastructure;
using NorthwestV2.Scenarios;
using NorthwestV2.Seed;

namespace NorthwestV2.Api;

public partial class Program
{
    private static readonly string[] AllowedOrigins = ["http://localhost:5173", "http://192.168.2.100:5173"];

    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        BuildPhase(builder);

        WebApplication app = builder.Build();
        await AppPhase(app, args);

        app.Run();
    }

    private static void BuildPhase(WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(5272); });

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddLogging();
        builder.Services.AddSwaggerGen(c =>
        {
            // https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
            string apiDocumentation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiDocumentation));

            // Must include all assemblies for which types exist
            string coreDocumentation = $"{typeof(NorthwestContext).Assembly.GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, coreDocumentation));
        });

        builder.Services.AddCoreServices(builder.Configuration);

        builder.Services.AddControllers().AddJsonOptions(o =>
        {
            // Allows frontend to generate union types
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddSeedServices(typeof(SeededCompany).Assembly);
        builder.Services.InstallScenarioLauncher();
        builder.Services.AddSession();
        builder.Services.AddDistributedMemoryCache();
    }

    private static async Task AppPhase(WebApplication app, string[] args)
    {
        app.UseRouting();
        app.UseCors(x =>
        {
            x.WithOrigins(AllowedOrigins);
            x.AllowAnyMethod();
            x.AllowAnyHeader();
            x.AllowCredentials(); // cookies allowed
        });
        app.UseSession();
        app.MapControllers();
        app.MapSwagger();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(a => { a.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1"); });

            // Development always starts from a clean database
            using IServiceScope scope = app.Services.CreateScope();
            await scope.ServiceProvider.GetRequiredService<NorthwestContext>().Database.EnsureDeletedAsync();
        }

        await app.Services.InitializeServices(app.Configuration);

        if (args.Contains("--seed") && args.Contains("--scenario"))
        {
            Console.WriteLine("Seeding database ! ");
            app.Services.ExecuteSeedFromSeedName(args.ElementAt(args.IndexOf("--seed") + 1));
        }
        else
        {
            app.Services.GetRequiredService<ILogger<Program>>()
                .LogInformation("Launching with seeds requires scenarios");
        }
    }
}
