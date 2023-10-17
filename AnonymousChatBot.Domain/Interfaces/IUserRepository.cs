using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetUserByIdAsync(long chatId);
    Task<List<Interest>> GetUserInterestsAsync(long chatId);
    Task<bool> IsUserInterestAsync(long chatId, int interestId);
    Task DeleteUserInterestAsync(long chatId, int interestId);
    Task AddInterestToUserAsync(long chatId, int interestId);
    Task DeleteUserAllInterestsAsync(long chatId);
    Task UpdateAsync(User user);
}