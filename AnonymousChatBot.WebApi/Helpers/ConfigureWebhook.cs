using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AnonymousChatBot.WebApi.Helpers;

public class ConfigureWebhook : IHostedService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public ConfigureWebhook(ILogger<ConfigureWebhook> logger, IServiceProvider services, IConfiguration configuration)
    {
        _logger = logger;
        _services = services;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAddress = @$"{_configuration["BotConfiguration:HostAddress"]}/bot";

        _logger.LogInformation($"Setting webhook: {webhookAddress}");

        await botClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        _logger.LogInformation("Removing webhook");

        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}