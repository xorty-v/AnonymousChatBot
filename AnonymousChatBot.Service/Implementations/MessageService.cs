using Telegram.Bot;
using Telegram.Bot.Types;
using AnonymousChatBot.Domain.Constans;
using AnonymousChatBot.Domain.Interfaces;

namespace AnonymousChatBot.Service.Implementations;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IInterestRepository _interestRepository;

    public MessageService(ITelegramBotClient botClient, IChatRepository chatRepository, IUserRepository userRepository,
        IInterestRepository interestRepository)
    {
        _botClient = botClient;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _interestRepository = interestRepository;
    }

    public async Task SendMessage(Message message)
    {
        var chatId = message.Chat.Id;
        var messageId = message.MessageId;

        var currentChat = await _chatRepository.GetChatByIdAsync(chatId);

        if (currentChat == null)
            await _botClient.SendTextMessageAsync(chatId, BotTextResponses.PARTNER_SEARCH);

        var comradeChatId = currentChat.ChatOne == chatId ? currentChat.ChatTwo : currentChat.ChatOne;

        await _botClient.CopyMessageAsync(comradeChatId, chatId, messageId);
    }

    public async Task EditMessage(Message message)
    {
        var chatId = message.Chat.Id;
        var messageId = message.MessageId + 1;
        var messageText = message.Text;

        var currentChat = await _chatRepository.GetChatByIdAsync(chatId);

        if (currentChat != null)
        {
            var comradeChatId = currentChat.ChatOne == chatId ? currentChat.ChatTwo : currentChat.ChatOne;

            await _botClient.EditMessageTextAsync(comradeChatId, messageId, messageText);
        }
    }

    public async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var messageId = callbackQuery.Message.MessageId;
        var queryData = callbackQuery.Data;

        if (queryData.StartsWith("interest_"))
        {
            var interestId = int.Parse(callbackQuery.Data.Replace("interest_", ""));

            var user = await _userRepository.GetUserByIdAsync(chatId);

            if (user != null && await _userRepository.IsUserInterestAsync(chatId, interestId))
            {
                await _userRepository.DeleteUserInterestAsync(chatId, interestId);
            }
            else
            {
                await _userRepository.AddInterestToUserAsync(chatId, interestId);
            }

            var userInterests = await _userRepository.GetUserInterestsAsync(chatId);
            var allInterests = await _interestRepository.GetAllAsync();
            var updatedKeyboard = BotKeyboardResponses.ChooseInterests(allInterests, userInterests);

            await _botClient.EditMessageReplyMarkupAsync(chatId, messageId, updatedKeyboard);
        }

        if (queryData == "reset")
        {
            await _userRepository.DeleteUserAllInterestsAsync(chatId);

            var userInterests = await _userRepository.GetUserInterestsAsync(chatId);
            var allInterests = await _interestRepository.GetAllAsync();
            var updatedKeyboard = BotKeyboardResponses.ChooseInterests(allInterests, userInterests);

            await _botClient.EditMessageReplyMarkupAsync(chatId, messageId, updatedKeyboard);
        }
    }
}