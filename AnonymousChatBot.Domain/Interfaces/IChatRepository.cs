using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IChatRepository
{
    Task AddAsync(Chat chat);
    Task<Chat> GetChatByIdAsync(long chatId);
    Task DeleteAsync(Chat chat);
}