using AnonymousChatBot.Domain.Constans;
using AnonymousChatBot.Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AnonymousChatBot.Domain.Entities.User;

namespace AnonymousChatBot.Service.Commands;

public class StartCommand : ICommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserRepository _userRepository;

    public StartCommand(ITelegramBotClient botClient, IUserRepository userRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
    }

    public string Name => "/start";

    public async Task Execute(Message message)
    {
        var chatId = message.Chat.Id;

        var user = await _userRepository.GetUserByIdAsync(chatId);

        if (user == null)
        {
            await _userRepository.AddAsync(new User() { ChatId = chatId });
            await _botClient.SendTextMessageAsync(chatId, BotTextResponses.WELCOME_MESSAGE);
        }
    }
}