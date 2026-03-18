using System.Reflection;
using System.Text.Json.Serialization;
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

        // builder.Services.AddCors(options =>
        // {
        //     options.AddPolicy("AllowAll", policy =>
        //     {
        //         policy
        //             .AllowAnyOrigin() // Cannot use AllowCredentials() with.WithOrigins 
        //             .AllowAnyHeader()
        //             .AllowAnyMethod(); // <-- allows PUT, POST, DELETE, etc.
        //         // .AllowCredentials();     // need this for cookies authentication 
        //     });
        // });
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5272); // listens on 0.0.0.0
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
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true; // Important!

            // Minimal dev setup
            // options.Cookie.SameSite = SameSiteMode.None; // needed for cross-origin
            // options.Cookie.SecurePolicy = CookieSecurePolicy.None; // allow HTTP
            options.Cookie.MaxAge = TimeSpan.FromHours(1); // IT WAS EXPIRING INSTANLY ! 
        });
        AppComposer.ComposeApplication(builder.Services, builder.Configuration);

        builder.Services.AddControllers().AddJsonOptions(o =>
        {
            // Allows frontend to generate union types 
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Seed & scenario

        builder.Services.AddSeedServices(typeof(SeededCompany).Assembly);
        builder.Services.InstallScenarioLauncher();

        WebApplication app = builder.Build();

        // app.UseCors("AllowAll"); // if the policy is not ran it won't ever work/.


        app.UseCors(x =>
        {
            x.WithOrigins(["http://localhost:5173"]);
            x.AllowAnyMethod();
            x.AllowAnyHeader();
            x.AllowCredentials(); // cookies allowed
        });
        app.UseRouting();
        app.UseSession();

        app.MapControllers();
        AppComposer.Initialize(app.Services);

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

        app.UseHttpsRedirection();

        app.Run();
    }
}


// Seed :

// Map each seed method to a route via reflection 