namespace AnonymousChatBot.Domain.Entities;

public class Interest
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users { get; set; }
}