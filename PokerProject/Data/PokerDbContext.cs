using Microsoft.EntityFrameworkCore;
using PokerProject.Models;

namespace PokerProject.Data
{
    public class PokerDbContext : DbContext
    {
        public PokerDbContext(DbContextOptions<PokerDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Score> Scores => Set<Score>();
        public DbSet<HallOfFame> HallOfFames => Set<HallOfFame>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Game nummer unik per session
            modelBuilder.Entity<Game>()
                .HasIndex(g => new { g.SessionId, g.GameNumber })
                .IsUnique();

            // Score unik per user per game
            modelBuilder.Entity<Score>()
                .HasIndex(s => new { s.UserId, s.GameId })
                .IsUnique();

           
        }
    }
}
