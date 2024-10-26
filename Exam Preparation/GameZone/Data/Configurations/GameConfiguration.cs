using GameZone.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameZone.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasOne<IdentityUser>(g => g.Publisher)
                .WithMany()
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne<Genre>(g => g.Genre)
                .WithMany(ge => ge.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
