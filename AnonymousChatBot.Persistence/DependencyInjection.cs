using AnonymousChatBot.Domain.Interfaces;
using AnonymousChatBot.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnonymousChatBot.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:Default"];
        services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

        services.AddScoped<IQueueRepository, QueueRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();

        return services;
    }
}