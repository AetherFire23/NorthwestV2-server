using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Infrastructure.Repositories;
using NorthwestV2.Practical;

namespace NorthwestV2.Infrastructure.Contexts;

public static class InfraInstaller
{
    public static void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IRoomRepository, RoomRepository>();
        serviceCollection.AddScoped<IGameRepository, GameRepository>();
        serviceCollection.AddScoped<ILobbyRepository, LobbyRepository>();
        serviceCollection.AddScoped<IGameRepository, GameRepository>();
        serviceCollection.AddScoped<IPlayerRepository, PlayerRepository>();
        serviceCollection.AddScoped<IProductionRepository, ProductionRepository>();
        serviceCollection.AddScoped<IItemrepository, ItemRepository>();
        serviceCollection.AddScoped<IGameLogsRepository, GameLogsRepository>();
    }
}