using Telegram.Bot.Types;

namespace AnonymousChatBot.Service.Implementations;

public interface IMessageService
{
    Task SendMessage(Message message);
    Task EditMessage(Message message);
    Task HandleCallbackQuery(CallbackQuery callbackQuery);
}