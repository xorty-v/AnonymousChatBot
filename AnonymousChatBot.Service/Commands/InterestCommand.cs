using AnonymousChatBot.Domain.Constans;
using AnonymousChatBot.Domain.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnonymousChatBot.Service.Commands;

public class InterestCommand : ICommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserRepository _userRepository;
    private readonly IInterestRepository _interestRepository;

    public InterestCommand(ITelegramBotClient botClient, IUserRepository userRepository,
        IInterestRepository interestRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _interestRepository = interestRepository;
    }

    public string Name => "/interests";

    public async Task Execute(Message message)
    {
        var chatId = message.Chat.Id;

        var userInterests = await _userRepository.GetUserInterestsAsync(chatId);
        var allInterests = await _interestRepository.GetAllAsync();

        await _botClient.SendTextMessageAsync(chatId, BotTextResponses.SELECT_INTERESTS,
            replyMarkup: BotKeyboardResponses.ChooseInterests(allInterests, userInterests));
    }
}