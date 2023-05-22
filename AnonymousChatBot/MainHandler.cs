using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using AnonymousChatBot.Resourses;

namespace AnonymousChatBot;

public class MainHandler
{
    private const string Start = "/start";
    private const string Next = "/next";
    private const string Stop = "/stop";

    private const string StartDialog = "Нажмите /next чтобы начать чат с незнакомцем";

    private const string UserFoundDialog = "Собеседник найден!\n\n" +
                                           "Чтобы остановить чат напишите /stop\n" +
                                           "Чтобы начать новый напишите /next\n\n" +
                                           "Переписуйтесь!";

    private const string UserLeftDialog = "Ваш собеседник прервал диалог, нажмите /next чтобы найти нового";
    private const string NoActiveChat = "Сначала подключите собеседника. Нажмите /next";
    private const string StopDialog = "Диалог остановлен. Чтобы начать новый нажмите /next";

    private readonly long _currentChatId;
    private readonly int _currentMessageId;
    private readonly MessageType _currentMessageType;
    private readonly string _currentMessageText;

    public MainHandler(Message message)
    {
        _currentChatId = message.Chat.Id;
        _currentMessageId = message.MessageId;
        _currentMessageType = message.Type;
        _currentMessageText = message.Text ?? string.Empty;
    }

    async public Task ProcessCommand(ITelegramBotClient botClient)
    {
        try
        {
            if (_currentMessageText.Contains(Start))
                await StartHandler(botClient);
            else if (_currentMessageText.Contains(Next))
                await NextHandler(botClient);
            else if (_currentMessageText.Contains(Stop))
                await StopHandler(botClient);
            else
                await MessageHandler(botClient);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProcessCommandError: {ex}");
        }
    }

    private async Task MessageHandler(ITelegramBotClient botClient)
    {
        //TODO: реализовать отображение пересылания сообщений
        var activeChatId = await DataBase.GetActiveChat(_currentChatId);

        if (activeChatId == 0)
            await botClient.SendTextMessageAsync(_currentChatId, NoActiveChat);

        switch (_currentMessageType)
        {
            case MessageType.Text:
            case MessageType.Audio:
            case MessageType.Voice:
            case MessageType.Photo:
            case MessageType.Video:
            case MessageType.VideoNote:
            case MessageType.Sticker:
                await botClient.CopyMessageAsync(activeChatId, _currentChatId, _currentMessageId);
                break;
        }
    }

    private async Task StartHandler(ITelegramBotClient botClient)
    {
        await botClient.SendTextMessageAsync(_currentChatId, StartDialog);
    }

    private async Task NextHandler(ITelegramBotClient botClient)
    {
        //TODO: реализовать метод для проверки подписок пользователя на определенные паблики перед началом работы
        var activeChatId = await DataBase.GetActiveChat(_currentChatId);

        if (activeChatId != 0)
        {
            await DataBase.DeleteChat(activeChatId);
            await botClient.SendTextMessageAsync(activeChatId, UserLeftDialog);
        }

        var chatTwo = await DataBase.GetUserFromQueue();

        if (chatTwo != 0 && chatTwo != _currentChatId)
        {
            await DataBase.DeleteUserFromQueue(chatTwo);
            await DataBase.CreateChat(_currentChatId, chatTwo);

            await botClient.SendTextMessageAsync(_currentChatId, UserFoundDialog);
            await botClient.SendTextMessageAsync(chatTwo, UserFoundDialog);
        }
        else
        {
            await DataBase.AddUserToQueue(_currentChatId);
        }
    }

    private async Task StopHandler(ITelegramBotClient botClient)
    {
        var activeChat = await DataBase.GetActiveChat(_currentChatId);

        if (activeChat != 0)
        {
            await DataBase.DeleteChat(_currentChatId);
            await botClient.SendTextMessageAsync(_currentChatId, StopDialog);
            await botClient.SendTextMessageAsync(activeChat, UserLeftDialog);
        }
    }
}