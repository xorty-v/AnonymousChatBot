namespace AnonymousChatBot.Domain.Constans;

public static class BotTextResponses
{
    public const string WELCOME_MESSAGE = "Добро пожаловать в анонимный чат бот!\n\n" +
                                          "/next — искать нового собеседника\n" +
                                          "/interests — добавить интересы поиска";

    public const string USER_FOUND_CHAT = "Собеседник найден 👋\n\n" +
                                          "/next — искать нового собеседника\n" +
                                          "/stop — закончить диалог\n\n" +
                                          "/interests — добавить интересы поиска";

    public const string USER_LEFT_CHAT = "Ваш собеседник прервал диалог 😞\n" +
                                         "нажмите /next чтобы найти нового";

    public const string PARTNER_SEARCH = "Напишите /next чтобы искать собеседника";

    public const string STOP_CHAT = "Диалог остановлен. Чтобы начать новый нажмите /next";

    public const string SELECT_INTERESTS = "Мы попытаемся соединить вас с собеседниками, которые выбрали похожие " +
                                           "интересы.\n\nВыберите ваши интересы:";
}