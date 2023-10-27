using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IQueueRepository
{
    Task AddAsync(Queue queue);
    Task<Queue> GetUserByInterestsAsync(List<Interest> interests);
    Task<bool> IsUserExistAsync(long chatId);
    Task DeleteAsync(Queue chat);
}