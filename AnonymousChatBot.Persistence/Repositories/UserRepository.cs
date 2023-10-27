using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddInterestToUserAsync(long chatId, int interestId)
    {
        var user = await _dbContext.Users
            .Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        var interest = await _dbContext.Interests
            .FirstOrDefaultAsync(i => i.Id == interestId);

        user.Interests.Add(interest);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> GetUserByIdAsync(long chatId)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.ChatId == chatId);
    }

    public async Task<List<Interest>> GetUserInterestsAsync(long chatId)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        return user.Interests.ToList();
    }

    public async Task<bool> IsUserInterestAsync(long chatId, int interestId)
    {
        return await _dbContext.Users.AsNoTracking()
            .AnyAsync(x => x.ChatId == chatId && x.Interests
                .Any(t => t.Id == interestId));
    }

    public async Task DeleteUserAllInterestsAsync(long chatId)
    {
        var user = await _dbContext.Users
            .Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        user.Interests.Clear();
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserInterestAsync(long chatId, int interestId)
    {
        var user = await _dbContext.Users.Include(u => u.Interests).FirstOrDefaultAsync(u => u.ChatId == chatId);
        var interest = user.Interests.FirstOrDefault(i => i.Id == interestId);

        user.Interests.Remove(interest);
        await _dbContext.SaveChangesAsync();
    }
}