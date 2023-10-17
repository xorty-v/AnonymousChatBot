using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence.Repositories;

public class QueueRepository : IQueueRepository
{
    private readonly ApplicationDbContext _dbContext;

    public QueueRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;


    public async Task CreateAsync(Queue queue)
    {
        await _dbContext.Queues.AddAsync(queue);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Queue> GetUserIdAsync()
    {
        return await _dbContext.Queues.Include(q => q.User).ThenInclude(q => q.Interests).FirstOrDefaultAsync();
    }

    public async Task<Queue> GetUserByInterestsAsync(List<Interest> interests)
    {
        return await _dbContext.Queues
            .Where(q => q.User.Interests.Any(ui => interests.Contains(ui)))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsUserExistAsync(long chatId)
    {
        return await _dbContext.Queues.AnyAsync(q => q.UserId == chatId);
    }

    public async Task DeleteAsync(Queue queue)
    {
        _dbContext.Queues.Remove(queue);
        await _dbContext.SaveChangesAsync();
    }
}