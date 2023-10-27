using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IInterestRepository
{
    Task<List<Interest>> GetAllAsync();
}