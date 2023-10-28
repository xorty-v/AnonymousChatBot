using AnonymousChatBot.Domain.Constans;
using AnonymousChatBot.Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AnonymousChatBot.Service.Commands;

public class LinkCommand : ICommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatRepository _chatRepository;

    public LinkCommand(ITelegramBotClient botClient, IChatRepository chatRepository)
    {
        _botClient = botClient;
        _chatRepository = chatRepository;
    }

    public string Name => "/link";

    public async Task Execute(Message message)
    {
        var userId = message.Chat.Id;
        var userName = message.From.Username;

        var currentChat = await _chatRepository.GetChatByIdAsync(userId);

        if (currentChat != null)
        {
            var comradeChatId = currentChat.ChatOne == userId ? currentChat.ChatTwo : currentChat.ChatOne;

            var profileLink = $"[Вот ссылка на мой телеграм-аккаунт](https://t.me/{userName})";
            
            await _botClient.SendTextMessageAsync(comradeChatId, profileLink, parseMode: ParseMode.Markdown,
                disableWebPagePreview: true);
        }
        else
        {
            await _botClient.SendTextMessageAsync(userId, BotTextResponses.PARTNER_SEARCH);
        }
    }
}