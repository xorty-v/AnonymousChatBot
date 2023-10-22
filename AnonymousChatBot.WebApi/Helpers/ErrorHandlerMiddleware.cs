using AnonymousChatBot.Service.Errors;
using Telegram.Bot.Exceptions;

namespace AnonymousChatBot.WebApi.Helpers;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ApiRequestException ex)
        {
            _logger.LogError($"[Telegram API Error]:\n[{ex.ErrorCode}]\n{ex.Message}");
        }
        catch (UnknownUpdateException ex)
        {
            _logger.LogError($"[Unknown update type]: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ErrorHandlerMiddleware]:\n{ex.Message}");
        }
    }
}