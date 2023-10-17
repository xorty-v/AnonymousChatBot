using Telegram.Bot.Types;

namespace AnonymousChatBot.Service.Interfaces;

public interface ICommandService
{
    public Task StartCommandService(Message message);
    public Task NextCommandService(Message message);
    public Task StopCommandService(Message message);
    public Task LinkCommandService(Message message);
    public Task InterestCommandService(Message message);

    /*public Task SettingsCommandService(Message message);*/
}