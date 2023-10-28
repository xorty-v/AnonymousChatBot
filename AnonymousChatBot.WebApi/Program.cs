using AnonymousChatBot.Persistence;
using AnonymousChatBot.Service;
using AnonymousChatBot.WebApi;
using AnonymousChatBot.WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddService();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();
app.MapControllers();
app.Run();