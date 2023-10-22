﻿using AnonymousChatBot.Service.Errors;
using AnonymousChatBot.Service.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AnonymousChatBot.Service.Implementations;

public class HandleUpdateService
{
    private readonly ICommandService _commandService;
    private readonly IMessageService _messageService;

    public HandleUpdateService(ICommandService commandService, IMessageService messageService)
    {
        _commandService = commandService;
        _messageService = messageService;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message!, cancellationToken),
            UpdateType.EditedMessage => BotOnEditedReceived(update.EditedMessage!, cancellationToken),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery, cancellationToken),
            _ => throw new UnknownUpdateException($"{update.Type}")
        };

        await handler;
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await _messageService.HandleCallbackQuery(callbackQuery);
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var userText = message.Text;

        switch (userText)
        {
            case "/start":
                await _commandService.StartCommandService(message);
                break;
            case "/next":
                await _commandService.NextCommandService(message);
                break;
            case "/stop":
                await _commandService.StopCommandService(message);
                break;
            case "/interests":
                await _commandService.InterestCommandService(message);
                break;
            case "/link":
                await _commandService.LinkCommandService(message);
                break;
            default:
                await _messageService.SendMessage(message);
                break;
        }
    }

    private async Task BotOnEditedReceived(Message message, CancellationToken cancellationToken)
    {
        await _messageService.EditMessage(message);
    }
}