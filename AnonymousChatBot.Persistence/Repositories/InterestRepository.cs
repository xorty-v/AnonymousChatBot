using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence.Repositories;

public class InterestRepository : IInterestRepository
{
    private readonly ApplicationDbContext _dbContext;

    public InterestRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<List<Interest>> GetAllAsync()
    {
        return await _dbContext.Interests.AsNoTracking().ToListAsync();
    }
}