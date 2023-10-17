using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IInterestRepository
{
    Task<Interest> GetInterestById(int interestId);
    Task<List<Interest>> GetAllAsync();
}