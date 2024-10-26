using GameZone.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameZone.Data.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder
                .HasData(
                    new Genre { Id = 1, Name = "Action" },
                    new Genre { Id = 2, Name = "Adventure" },
                    new Genre { Id = 3, Name = "Fighting" },
                    new Genre { Id = 4, Name = "Sports" },
                    new Genre { Id = 5, Name = "Racing" },
                    new Genre { Id = 6, Name = "Strategy" });
        }
    }
}
