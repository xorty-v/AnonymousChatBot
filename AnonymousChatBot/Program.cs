using AnonymousChatBot;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var config = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();
var token = config["Telegram:Token"];

TelegramBotClient client = new TelegramBotClient(token);

Console.WriteLine("BOT is working!");
client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
Console.ReadLine();

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exceptinon, CancellationToken token)
{
    var errorMessage = exceptinon switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n {apiRequestException.ErrorCode} \n" +
                                                   $"{apiRequestException.Message}",
        _ => exceptinon.ToString()
    };

    Console.WriteLine($"Error: {errorMessage}");
    return Task.CompletedTask;
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    if (IsUpdateCorrectType(update))
    {
        ServerNotification(update.Message); // message in console.
        MainHandler commandHandler = new MainHandler(update.Message);
        await commandHandler.ProcessCommand(botClient);
    }
}

void ServerNotification(Message message)
{
    var text = message.Text;
    var type = message.Type;
    Console.Write($"<{message.From.Username ?? message.From.FirstName}> is wrote:   ");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"{text}   ");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"messageType: {type}");
    Console.ResetColor();
    Console.WriteLine(new string('-', 20));
}

bool IsUpdateCorrectType(Update update)  
{
    if (!update.Type.Equals(UpdateType.Message))       
        return false;

    return true;
}

























// var botClient = new TelegramBotClient("5987906858:AAGl9ie-kvNOw-tfZAfV42ZmiWzjOLLGjPg");
// var _dataBase = new DataBaseContext();
//
// using var cts = new CancellationTokenSource();
//
// var receiverOptions = new ReceiverOptions
// {
//     AllowedUpdates = { }
// };
//
// botClient.StartReceiving(
//     HandleUpdateAsync,
//     HandlePollingErrorAsync,
//     receiverOptions,
//     cts.Token);
//
// var me = await botClient.GetMeAsync();
// Console.WriteLine($"Слушаем: {me.Username}");
// Console.ReadLine();
//
// cts.Cancel();
//
// async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
// {
//     if (update.Type == UpdateType.Message && update?.Message?.Text != null)
//     {
//         DataBase.Test(update.Message);
//     }
// }
//
// async Task HandleMessage(ITelegramBotClient botClient, Message message)
// {
//
//     var chatId = message.Chat.Id;
//
//     if (message.Text == "/next")
//     {
//         var chatTwo = await GetChatFromQueue();
//
//         if (chatTwo != 0 && chatTwo != chatId)
//         {
//             await DeleteQueue(chatTwo);
//             await CreateChat(chatId, chatTwo);
//             await botClient.SendTextMessageAsync(chatId,
//                 "Собеседник найден!\n\nЧтобы остановить чат напишите /stop" +
//                 "\nЧтобы начать новый напишите /next \n\nПереписуйтесь!");
//             await botClient.SendTextMessageAsync(chatTwo,
//                 "Собеседник найден!\n\nЧтобы остановить чат напишите /stop" +
//                 "\nЧтобы начать новый напишите /next \n\nПереписуйтесь!");
//         }
//         else
//         {
//             await AddQueue(chatId);
//         }
//
//         return;
//     }
//
//     var person = await GetActiveChat(chatId);
//
//     if (message.Text == "/stop")
//     {
//         if (person != 0)
//         {
//             await DeleteChat(chatId);
//             await botClient.SendTextMessageAsync(chatId, "Диалог остановлен. Чтобы начать новый нажмите /next");
//             await botClient.SendTextMessageAsync(person, "Ваш собеседник прервал диалог. Чтобы начать новый нажмите /next");
//         }
//
//         return;
//     }
//
//     if (person != 0)
//     {
//         if (message.Type == MessageType.Voice)
//         {
//             await botClient.CopyMessageAsync(chatId, chatId, message.MessageId);
//         }
//
//         await botClient.SendTextMessageAsync(person, message.Text);
//     }
//     else
//     {
//         await botClient.SendTextMessageAsync(chatId, "Сначала подключите собеседника. Нажмите /next");
//     }
// }
//
// Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
// {
//     var ErrorMessage = exception switch
//     {
//         ApiRequestException apiRequestException
//             => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
//         _ => exception.ToString()
//     };
//
//     Console.WriteLine(ErrorMessage);
//     return Task.CompletedTask;
// }

// #region Логика Очереди
//
// async Task AddQueue(long chatId)
// {
//     var queue = await _dataBase.Queues.FirstOrDefaultAsync(q => q.ChatId == chatId);
//     if (queue == null)
//     {
//         await _dataBase.Queues.AddAsync(new Queue { ChatId = chatId });
//         await _dataBase.SaveChangesAsync();
//     }
// }
//
// async Task DeleteQueue(long chatId)
// {
//     var queue = await _dataBase.Queues.FirstOrDefaultAsync(q => q.ChatId == chatId);
//     if (queue != null)
//     {
//         _dataBase.Queues.Remove(queue);
//         await _dataBase.SaveChangesAsync();
//     }
// }
//
// async Task<long> GetChatFromQueue()
// {
//     return await _dataBase.Queues.Select(q => q.ChatId).FirstOrDefaultAsync();
// }
//
// #endregion
//
// #region Логика Чата
//
// async Task CreateChat(long chatOne, long chatTwo)
// {
//     await _dataBase.Chats.AddAsync(new Chat { ChatOne = chatOne, ChatTwo = chatTwo });
//     await _dataBase.SaveChangesAsync();
// }
//
// async Task DeleteChat(long chatId)
// {
//     var chat = await _dataBase.Chats.FirstOrDefaultAsync(c => c.ChatOne == chatId || c.ChatTwo == chatId);
//     if (chat != null)
//     {
//         _dataBase.Chats.Remove(chat);
//         await _dataBase.SaveChangesAsync();
//     }
// }
//
// async Task<long> GetActiveChat(long chatId)
// {
//     var chat = await _dataBase.Chats.FirstOrDefaultAsync(c => c.ChatOne == chatId || c.ChatTwo == chatId);
//
//     if (chat != null)
//     {
//         if (chat.ChatOne == chatId)
//         {
//             return chat.ChatTwo;
//         }
//         else if (chat.ChatTwo == chatId)
//         {
//             return chat.ChatOne;
//         }
//     }
//
//     return 0;
// }
//
// #endregion