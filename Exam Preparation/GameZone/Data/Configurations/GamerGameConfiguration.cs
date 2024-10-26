using GameZone.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameZone.Data.Configurations
{
    public class GamerGameConfiguration : IEntityTypeConfiguration<GamerGame>
    {
        public void Configure(EntityTypeBuilder<GamerGame> builder)
        {
            builder.HasKey(gg => new { gg.GameId, gg.GamerId });

            builder
                .HasOne<IdentityUser>(gg => gg.Gamer)
                .WithMany()
                .HasForeignKey(g => g.GamerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne<Game>(gg => gg.Game)
                .WithMany(g => g.GamersGames)
                .HasForeignKey(g => g.GameId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
