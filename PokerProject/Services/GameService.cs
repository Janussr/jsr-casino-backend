using PokerProject.Data;
using PokerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace PokerProject.Services
{
    public interface IGameService
    {
        Task<Game> StartGameAsync();
        Task<Score> AddScoreAsync(int gameId, int userId, int value);
        Task<Game> EndGameAsync(int gameId);
        Task<List<Game>> GetAllGamesAsync();
        Task<Game?> GetGameByIdAsync(int gameId);
    }


    public class GameService : IGameService
    {
        private readonly PokerDbContext _context;

        public GameService(PokerDbContext context)
        {
            _context = context;
        }

        // Start a new game
        public async Task<Game> StartGameAsync()
        {
            // Find næste GameNumber
            int nextGameNumber = await _context.Games.AnyAsync()
                ? await _context.Games.MaxAsync(g => g.GameNumber) + 1
                : 1;

            var game = new Game
            {
                GameNumber = nextGameNumber,
                StartedAt = DateTime.UtcNow,
                IsFinished = false
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return game;
        }

        // Add score for a player in a game
        public async Task<Score> AddScoreAsync(int gameId, int userId, int value)
        {
            var score = new Score
            {
                GameId = gameId,
                UserId = userId,
                Value = value,
                CreatedAt = DateTime.UtcNow
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return score;
        }

        // End game, calculate winner and update HallOfFame
        public async Task<Game> EndGameAsync(int gameId)
        {
            var game = await _context.Games
                .Include(g => g.Scores)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new Exception("Game not found");

            if (game.IsFinished)
                throw new Exception("Game already finished");

            // Calculate winner (highest total)
            var winnerGroup = game.Scores
                .GroupBy(s => s.UserId)
                .Select(g => new { UserId = g.Key, Total = g.Sum(s => s.Value) })
                .OrderByDescending(g => g.Total)
                .FirstOrDefault();

            if (winnerGroup != null)
            {
                var hall = new HallOfFame
                {
                    GameId = game.Id,
                    UserId = winnerGroup.UserId,
                    WinningScore = winnerGroup.Total,
                    WinDate = DateTime.UtcNow
                };

                _context.HallOfFames.Add(hall);
            }

            game.IsFinished = true;
            game.EndedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return game;
        }

        // Get all games
        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _context.Games
                .Include(g => g.Scores)
                .ThenInclude(s => s.User)
                .Include(g => g.Winner)
                .ThenInclude(h => h.User)
                .ToListAsync();
        }

        // Get a single game
        public async Task<Game?> GetGameByIdAsync(int gameId)
        {
            return await _context.Games
                .Include(g => g.Scores)
                .ThenInclude(s => s.User)
                .Include(g => g.Winner)
                .ThenInclude(h => h.User)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }
    }
}
