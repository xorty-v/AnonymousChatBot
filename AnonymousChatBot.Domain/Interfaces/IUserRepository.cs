using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task AddInterestToUserAsync(long chatId, int interestId);
    Task<User> GetUserByIdAsync(long chatId);
    Task<List<Interest>> GetUserInterestsAsync(long chatId);
    Task<bool> IsUserInterestAsync(long chatId, int interestId);
    Task DeleteUserInterestAsync(long chatId, int interestId);
    Task DeleteUserAllInterestsAsync(long chatId);
}