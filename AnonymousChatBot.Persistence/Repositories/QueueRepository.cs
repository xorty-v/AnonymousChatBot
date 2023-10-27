using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence.Repositories;

public class QueueRepository : IQueueRepository
{
    private readonly ApplicationDbContext _dbContext;

    public QueueRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;


    public async Task AddAsync(Queue queue)
    {
        await _dbContext.Queues.AddAsync(queue);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Queue> GetUserByInterestsAsync(List<Interest> interests)
    {
        return interests.Count == 0
            ? await _dbContext.Queues.AsNoTracking().FirstOrDefaultAsync(u => u.User.Interests.Count == 0)
            : await _dbContext.Queues.AsNoTracking()
                .FirstOrDefaultAsync(q => q.User.Interests.Any(u => interests.Contains(u)));
    }

    public async Task<bool> IsUserExistAsync(long chatId)
    {
        return await _dbContext.Queues.AsNoTracking().AnyAsync(q => q.UserId == chatId);
    }

    public async Task DeleteAsync(Queue queue)
    {
        _dbContext.Queues.Remove(queue);
        await _dbContext.SaveChangesAsync();
    }
}