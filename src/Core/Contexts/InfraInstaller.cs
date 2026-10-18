using Microsoft.Extensions.DependencyInjection;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Repositories;

namespace NorthwestV2.Contexts;

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