using Telegram.Bot;
using Telegram.Bot.Types;
using AnonymousChatBot.Domain;
using Telegram.Bot.Types.Enums;
using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using AnonymousChatBot.Service.Interfaces;
using Chat = AnonymousChatBot.Domain.Entities.Chat;
using User = AnonymousChatBot.Domain.Entities.User;

namespace AnonymousChatBot.Service.Implementations;

public class CommandService : ICommandService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatRepository _chatRepository;
    private readonly IQueueRepository _queueRepository;
    private readonly IInterestRepository _interestRepository;
    private readonly IUserRepository _userRepository;

    public CommandService(ITelegramBotClient botClient, IChatRepository chatRepository,
        IQueueRepository queueRepository, IUserRepository userRepository, IInterestRepository interestRepository)
    {
        _botClient = botClient;
        _chatRepository = chatRepository;
        _queueRepository = queueRepository;
        _userRepository = userRepository;
        _interestRepository = interestRepository;
    }

    public async Task StartCommandService(Message message)
    {
        var chatId = message.Chat.Id;

        var user = await _userRepository.GetUserByIdAsync(chatId);

        if (user == null)
        {
            await _userRepository.CreateAsync(new User() { ChatId = chatId });
            await _botClient.SendTextMessageAsync(chatId, BotTextResponses.WELCOME_MESSAGE);
        }
        else
        {
            await NextCommandService(message);
        }
    }

    public async Task NextCommandService(Message message)
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
        Queue peopleFromQueue;

        if (userInterests.Count == 0)
        {
            peopleFromQueue = await _queueRepository.GetUserIdAsync();
        }
        else
        {
            peopleFromQueue = await _queueRepository.GetUserByInterestsAsync(userInterests);
        }

        if (peopleFromQueue != null && peopleFromQueue.UserId != userId)
        {
            await _queueRepository.DeleteAsync(peopleFromQueue);
            await _chatRepository.CreateAsync(new Chat() { ChatOne = userId, ChatTwo = peopleFromQueue.UserId });

            await _botClient.SendTextMessageAsync(userId, BotTextResponses.USER_FOUND_CHAT);
            await _botClient.SendTextMessageAsync(peopleFromQueue.UserId, BotTextResponses.USER_FOUND_CHAT);
        }
        else
        {
            if (!await _queueRepository.IsUserExistAsync(userId))
            {
                await _queueRepository.CreateAsync(new Queue() { UserId = userId });
            }
        }
    }

    public async Task StopCommandService(Message message)
    {
        var currentChatId = message.Chat.Id;

        var currentChat = await _chatRepository.GetChatByIdAsync(currentChatId);

        if (currentChat != null)
        {
            await _chatRepository.DeleteAsync(currentChat);

            await _botClient.SendTextMessageAsync(currentChatId, BotTextResponses.STOP_CHAT);

            var comradeChatId = currentChat.ChatOne == currentChatId ? currentChat.ChatTwo : currentChat.ChatOne;

            await _botClient.SendTextMessageAsync(comradeChatId, BotTextResponses.USER_LEFT_CHAT);
        }
        else
        {
            await _botClient.SendTextMessageAsync(currentChatId, BotTextResponses.PARTNER_SEARCH);
        }
    }

    public async Task LinkCommandService(Message message)
    {
        var userId = message.Chat.Id;

        var currentChat = await _chatRepository.GetChatByIdAsync(userId);

        if (currentChat != null)
        {
            var comradeChatId = currentChat.ChatOne == userId ? currentChat.ChatTwo : currentChat.ChatOne;

            var profileLink = $"[Вот ссылка на мой телеграм-аккаунт](https://t.me/{message.From.Username})";
            await _botClient.SendTextMessageAsync(comradeChatId, profileLink, parseMode: ParseMode.Markdown,
                disableWebPagePreview: true);
        }
        else
        {
            await _botClient.SendTextMessageAsync(userId, BotTextResponses.PARTNER_SEARCH);
        }
    }

    public async Task InterestCommandService(Message message)
    {
        var chatId = message.Chat.Id;

        var userInterests = await _userRepository.GetUserInterestsAsync(chatId);
        var allInterests = await _interestRepository.GetAllAsync();

        await _botClient.SendTextMessageAsync(chatId, BotTextResponses.SELECT_INTERESTS,
            replyMarkup: BotKeyboardResponses.ChooseInterests(allInterests, userInterests));
    }
}