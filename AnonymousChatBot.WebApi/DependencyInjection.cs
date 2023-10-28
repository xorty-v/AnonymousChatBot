using AnonymousChatBot.WebApi.Helpers;
using Telegram.Bot;

namespace AnonymousChatBot.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["BotConfiguration:BotToken"];

        services.AddHttpClient("TelegramWebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient =>
                new TelegramBotClient(connectionString, httpClient));

        services.AddHostedService<ConfigureWebhook>();
        services.AddScoped<HandleUpdate>();

        return services;
    }
}