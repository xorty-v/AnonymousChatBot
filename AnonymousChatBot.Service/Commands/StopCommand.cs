using AnonymousChatBot.Domain.Constans;
using AnonymousChatBot.Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnonymousChatBot.Service.Commands;

public class StopCommand : ICommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatRepository _chatRepository;

    public StopCommand(ITelegramBotClient botClient, IChatRepository chatRepository)
    {
        _botClient = botClient;
        _chatRepository = chatRepository;
    }

    public string Name => "/stop";

    public async Task Execute(Message message)
    {
        var currentChatId = message.Chat.Id;

        var currentChat = await _chatRepository.GetChatByIdAsync(currentChatId);

        if (currentChat != null)
        {
            await _chatRepository.DeleteAsync(currentChat);
            var comradeChatId = currentChat.ChatOne == currentChatId ? currentChat.ChatTwo : currentChat.ChatOne;

            await _botClient.SendTextMessageAsync(currentChatId, BotTextResponses.STOP_CHAT);
            await _botClient.SendTextMessageAsync(comradeChatId, BotTextResponses.USER_LEFT_CHAT);
        }
        else
        {
            await _botClient.SendTextMessageAsync(currentChatId, BotTextResponses.PARTNER_SEARCH);
        }
    }
}