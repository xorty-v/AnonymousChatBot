using AnonymousChatBot.Service.Commands;
using AnonymousChatBot.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace AnonymousChatBot.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<ICommand, InterestCommand>();
        services.AddScoped<ICommand, LinkCommand>();
        services.AddScoped<ICommand, NextCommand>();
        services.AddScoped<ICommand, StartCommand>();
        services.AddScoped<ICommand, StopCommand>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}