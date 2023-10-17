using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> GetUserByIdAsync(long chatId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
    }

    public async Task<List<Interest>> GetUserInterestsAsync(long chatId)
    {
        var user = await _dbContext.Users.Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        return user.Interests.ToList();
    }

    public async Task<bool> IsUserInterestAsync(long chatId, int interestId)
    {
        return await _dbContext.Users
            .AnyAsync(x => x.ChatId == chatId && x.Interests
                .Any(t => t.Id == interestId));
    }

    public async Task AddInterestToUserAsync(long chatId, int interestId)
    {
        var user = await _dbContext.Users.Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        if (user == null) return;

        var interest = await _dbContext.Interests.FirstOrDefaultAsync(i => i.Id == interestId);

        if (interest != null)
        {
            user.Interests.Add(interest);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteUserAllInterestsAsync(long chatId)
    {
        var user = await _dbContext.Users.Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        if (user == null) return;

        user.Interests.Clear();
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserInterestAsync(long chatId, int interestId)
    {
        var user = await _dbContext.Users.Include(u => u.Interests).FirstOrDefaultAsync(u => u.ChatId == chatId);

        if (user == null) return;

        var interest = user.Interests.FirstOrDefault(i => i.Id == interestId);

        if (interest != null)
        {
            user.Interests.Remove(interest);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}