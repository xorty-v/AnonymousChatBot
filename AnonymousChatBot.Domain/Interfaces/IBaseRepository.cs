using AnonymousChatBot.Domain.Entities;

namespace AnonymousChatBot.Domain.Interfaces;

public interface IBaseRepository<T>
{
    Task CreateAsync(T entity);
    Task DeleteAsync(T entity);
}