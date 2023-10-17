namespace AnonymousChatBot.Domain.Entities;

public class User
{
    public long ChatId { get; set; }
    public Queue Queue { get; set; }

    public ICollection<Interest> Interests { get; set; }
}