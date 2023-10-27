using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ChatRepository(ApplicationDbContext dbContext) =>
        _dbContext = dbContext;


    public async Task AddAsync(Chat chat)
    {
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Chat> GetChatByIdAsync(long chatId)
    {
        return await _dbContext.Chats.AsNoTracking()
            .FirstOrDefaultAsync(c => c.ChatOne == chatId || c.ChatTwo == chatId);
    }

    public async Task DeleteAsync(Chat chat)
    {
        _dbContext.Chats.Remove(chat);
        await _dbContext.SaveChangesAsync();
    }
}