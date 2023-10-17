using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IQueueRepository : IBaseRepository<Queue>
{
    Task<Queue> GetUserIdAsync();
    Task<Queue> GetUserByInterestsAsync(List<Interest> interests);
    Task<bool> IsUserExistAsync(long chatId);
}