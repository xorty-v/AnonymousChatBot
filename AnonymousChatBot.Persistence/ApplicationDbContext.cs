using AnonymousChatBot.Domain.Entities;
using AnonymousChatBot.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AnonymousChatBot.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) => Database.Migrate();

    public DbSet<User> Users { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Queue> Queues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new InterestConfiguration());
        builder.ApplyConfiguration(new ChatConfiguration());
        builder.ApplyConfiguration(new QueueConfiguration());

        base.OnModelCreating(builder);
    }
}