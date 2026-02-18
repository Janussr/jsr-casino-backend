using Microsoft.EntityFrameworkCore;
using PokerProject.Models;

namespace PokerProject.Data
{
    public class PokerDbContext : DbContext
    {
        public PokerDbContext(DbContextOptions<PokerDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Score> Scores => Set<Score>();
        public DbSet<HallOfFame> HallOfFames => Set<HallOfFame>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Game nummer unik globalt (eller pr. spil, hvis du vil ændre senere)
            modelBuilder.Entity<Game>()
                .HasIndex(g => g.GameNumber)
                .IsUnique();

            // Score unik per user per game
            modelBuilder.Entity<Score>()
                .HasIndex(s => new { s.UserId, s.GameId })
                .IsUnique();

            // Relations
            modelBuilder.Entity<User>()
                .HasMany(u => u.Scores)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.HallOfFames)
                .WithOne(h => h.User)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Scores)
                .WithOne(s => s.Game)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
    .HasOne(g => g.Winner)
    .WithOne(h => h.Game)
    .HasForeignKey<HallOfFame>(h => h.GameId)
    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
