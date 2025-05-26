using FCG.FiapCloudGames.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Game");
            builder.HasKey(g => g.GameId);
            builder.Property(g => g.GameId).ValueGeneratedOnAdd();
            builder.Property(g => g.GameId).HasColumnType("INT");

            builder.Property(g => g.Name).IsRequired().HasMaxLength(50);
            builder.Property(g => g.Description).IsRequired().HasMaxLength(100);
            builder.Property(g => g.Genre).IsRequired().HasMaxLength(30);
            builder.Property(g => g.ReleaseDate).HasColumnType("DATE");
            builder.Property(g => g.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(g => g.UpdatedAt);
        }
    }
}
