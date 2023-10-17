using AnonymousChatBot.Persistence;
using AnonymousChatBot.WebApi;
using AnonymousChatBot.WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("TelegramWebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
        new TelegramBotClient(builder.Configuration["BotConfiguration:BotToken"], httpClient));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();
app.MapControllers();
app.Run();