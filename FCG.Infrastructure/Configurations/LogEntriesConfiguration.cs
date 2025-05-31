using FCG.Domain.Entities;
using FCG.FiapCloudGames.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations
{
    public class LogEntriesConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.ToTable("Log_entry");
            builder.HasKey(x => x.LogId);
            builder.Property(log => log.LogId).
                    IsRequired();
            builder.Property(log => log.Timestamp)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(log => log.Level)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(log => log.Message)
                   .IsRequired()
                   .HasMaxLength(1000);
            builder.Property(log => log.StackTrace)
                   .HasMaxLength(4000);
        }
    }
}
