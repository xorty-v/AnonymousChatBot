using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using AnonymousChatBot.Service.Commands;
using AnonymousChatBot.Service.Implementations;

namespace AnonymousChatBot.WebApi.Helpers;

public class HandleUpdate
{
    private readonly IEnumerable<ICommand> _commands;
    private readonly IMessageService _messageService;

    public HandleUpdate(IEnumerable<ICommand> commands, IMessageService messageService)
    {
        _commands = commands;
        _messageService = messageService;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message!, cancellationToken),
            UpdateType.EditedMessage => BotOnEditedReceived(update.EditedMessage!, cancellationToken),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery, cancellationToken),
            _ => throw new Exception($"UnknownUpdate: {update.Type}")
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var command = _commands.FirstOrDefault(command => command.Name == message.Text);

        if (command != null)
            await command.Execute(message);
        else
            await _messageService.SendMessage(message);
    }

    private async Task BotOnEditedReceived(Message message, CancellationToken cancellationToken)
    {
        await _messageService.EditMessage(message);
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await _messageService.HandleCallbackQuery(callbackQuery);
    }
}