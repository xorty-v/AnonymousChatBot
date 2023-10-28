using AnonymousChatBot.Domain.Constans;
using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Chat = AnonymousChatBot.Domain.Entities.Chat;

namespace AnonymousChatBot.Service.Commands;

public class NextCommand : ICommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IQueueRepository _queueRepository;

    public NextCommand(ITelegramBotClient botClient, IChatRepository chatRepository, IUserRepository userRepository,
        IQueueRepository queueRepository)
    {
        _botClient = botClient;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _queueRepository = queueRepository;
    }

    public string Name => "/next";

    public async Task Execute(Message message)
    {
        var userId = message.Chat.Id;

        var currentChat = await _chatRepository.GetChatByIdAsync(userId);

        if (currentChat != null)
        {
            var comradeChatId = currentChat.ChatOne == userId ? currentChat.ChatTwo : currentChat.ChatOne;

            await _chatRepository.DeleteAsync(currentChat);
            await _botClient.SendTextMessageAsync(comradeChatId, BotTextResponses.USER_LEFT_CHAT);
        }

        var userInterests = await _userRepository.GetUserInterestsAsync(userId);
        var nextUser = await _queueRepository.GetUserByInterestsAsync(userInterests);

        if (nextUser != null && nextUser.UserId != userId)
        {
            await _queueRepository.DeleteAsync(nextUser);
            await _chatRepository.AddAsync(new Chat() { ChatOne = userId, ChatTwo = nextUser.UserId });

            await _botClient.SendTextMessageAsync(userId, BotTextResponses.USER_FOUND_CHAT);
            await _botClient.SendTextMessageAsync(nextUser.UserId, BotTextResponses.USER_FOUND_CHAT);
        }
        else
        {
            if (!await _queueRepository.IsUserExistAsync(userId))
            {
                await _queueRepository.AddAsync(new Queue() { UserId = userId });
            }
        }
    }
}