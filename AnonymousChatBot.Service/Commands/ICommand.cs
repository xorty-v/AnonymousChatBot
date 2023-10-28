using Telegram.Bot.Types;

namespace AnonymousChatBot.Service.Commands;

public interface ICommand
{
    public string Name { get; }

    public Task Execute(Message message);
}