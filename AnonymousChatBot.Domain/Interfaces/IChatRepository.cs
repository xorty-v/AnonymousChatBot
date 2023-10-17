using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IChatRepository : IBaseRepository<Chat>
{
    Task<Chat> GetChatByIdAsync(long chatId);
}