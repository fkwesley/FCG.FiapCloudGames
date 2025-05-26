using FCG.FiapCloudGames.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Context
{
    public class FiapCloudGamesDbContext : DbContext
    {
        private readonly string _connectionString;

        //public FiapCloudGamesDbContext()
        //{
        //}

        public FiapCloudGamesDbContext(DbContextOptions<FiapCloudGamesDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Game { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FiapCloudGamesDbContext).Assembly);

            // Alternatively, you can apply configurations explicitly if needed
            //modelBuilder.ApplyConfiguration(new GameConfiguration());
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
