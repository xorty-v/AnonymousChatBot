using AnonymousChatBot.Domain.Interfaces;
using AnonymousChatBot.Persistence.Repositories;
using AnonymousChatBot.Service.Implementations;
using AnonymousChatBot.Service.Interfaces;
using AnonymousChatBot.WebApi.Helpers;

namespace AnonymousChatBot.WebApi;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddHostedService<ConfigureWebhook>();
        services.AddScoped<HandleUpdateService>();

        services.AddScoped<ICommandService, CommandService>();
        services.AddScoped<IMessageService, MessageService>();
    }
}