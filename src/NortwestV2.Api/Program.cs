using System.Reflection;
using AetherFire23.Commons.Scenarios;
using AetherFire23.Commons.Seeding;
using AetherFire23.ERP.Domain;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Application.Installation;
using NorthwestV2.Compose;
using NorthwestV2.Infrastructure;
using NorthwestV2.Infrastructure.Contexts;
using NorthwestV2.Seed;

namespace NortwestV2.Api;

public partial class Program
{
    //TODO: put it inside config ?
    public static readonly string FRONTEND_URL = "http://localhost:5173";

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod(); // <-- allows PUT, POST, DELETE, etc.
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/openapi-comments?view=aspnetcore-10.0
            // https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            // Must include all assemblies for which types exist
            var domainDocumentation = $"{typeof(ActionWithTargetsAvailability).Assembly.GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, domainDocumentation));

            // c.SchemaFilter<RemoveNullableSchemaFilter>();
        });
        builder.Services.AddLogging();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        AppComposer.ComposeApplication(builder.Services, builder.Configuration);

        builder.Services.AddControllers();

        // Seed & scenario

        builder.Services.AddSeedServices(typeof(SeededCompany).Assembly);
        builder.Services.InstallScenarioLauncher();

        WebApplication app = builder.Build();

        app.UseCors("AllowAll");
        app.UseSession();

        AppComposer.Initialize(app.Services);

        app.MapControllers();
        app.MapSwagger();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(a => { a.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1"); });

            // Seed and Scenario

            // Deletes database after migrating it. 
            using (var scope = app.Services.CreateScope())
            {
                var s = scope.ServiceProvider.GetRequiredService<NorthwestContext>();

                // Deletes the database, tables, schemas
                s.Database.EnsureDeleted();

                // Re-creates the schemas, tables, 
                s.Database.Migrate();
            }

            if (args.Contains("--seed") && args.Contains("--scenario"))
            {
                app.Services.ExecuteSeedFromSeedName(args.ElementAt(args.IndexOf("--seed") + 1));
                // Leave as fire-and-forget async call. 
                // app.Services.LaunchScenarioBrowser(args[args.IndexOf("--scenario") + 1]);
            }
            else
            {
                app.Services.GetRequiredService<ILogger<Program>>()
                    .LogInformation("Launching with seeds requires scenarios");
            }
        }

        app.UseCors(x =>
        {
            x.AllowAnyOrigin();
            x.AllowAnyHeader();
        });

        app.UseHttpsRedirection();

        app.Run();
    }
}


// Seed :

// Map each seed method to a route via reflection 