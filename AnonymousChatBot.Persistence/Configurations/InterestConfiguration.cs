using AnonymousChatBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousChatBot.Persistence.Configurations;

public class InterestConfiguration : IEntityTypeConfiguration<Interest>
{
    public void Configure(EntityTypeBuilder<Interest> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.Property(i => i.Name).HasMaxLength(20);

        builder.HasData(
            new Interest[]
            {
                new Interest { Id = 1, Name = "IT" },
                new Interest { Id = 2, Name = "Спорт" },
                new Interest { Id = 3, Name = "Фильмы" },
                new Interest { Id = 4, Name = "Музыка" },
                new Interest { Id = 5, Name = "Книги" },
                new Interest { Id = 6, Name = "Мемы" },
                new Interest { Id = 7, Name = "Одиночесво" },
                new Interest { Id = 8, Name = "Аниме" },
            });
    }
}