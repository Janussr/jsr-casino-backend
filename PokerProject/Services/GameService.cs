using Microsoft.EntityFrameworkCore;
using PokerProject.Data;
using PokerProject.DTOs;
using PokerProject.Models;

namespace PokerProject.Services
{
    public interface IGameService
    {
        Task<GameDto> StartGameAsync();
        Task<ScoreDto> AddScoreAsync(int gameId, int userId, int value);
        Task<GameDto> EndGameAsync(int gameId);
        Task<List<GameDto>> GetAllGamesAsync();
        Task<GameDto?> GetGameByIdAsync(int gameId);
        Task AddParticipantsAsync(int gameId, List<int> userIds);
        Task<List<ParticipantDto>> GetParticipantsAsync(int gameId);
        Task<bool> IsUserParticipantAsync(int gameId, int userId);
    }


    public class GameService : IGameService
    {
        private readonly PokerDbContext _context;

        public GameService(PokerDbContext context)
        {
            _context = context;
        }

        // Start a new game
        public async Task<GameDto> StartGameAsync()
        {
            var game = new Game
            {
                GameNumber = await GetNextGameNumber(),
                StartedAt = DateTime.UtcNow,
                IsFinished = false
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return new GameDto
            {
                Id = game.Id,
                GameNumber = game.GameNumber,
                StartedAt = game.StartedAt,
                IsFinished = game.IsFinished
            };
        }

        private async Task<int> GetNextGameNumber()
        {
            if (!await _context.Games.AnyAsync())
                return 1;

            return await _context.Games.MaxAsync(g => g.GameNumber) + 1;
        }



        // Add score for a player in a game
        public async Task<ScoreDto> AddScoreAsync(int gameId, int userId, int points)
        {
            var score = new Score
            {
                GameId = gameId,
                UserId = userId,
                Points = points,
                CreatedAt = DateTime.UtcNow
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return new ScoreDto
            {
                UserId = score.UserId,
                Points = score.Points,
            };
        }


        // End game, calculate winner and update HallOfFame
        public async Task<GameDto> EndGameAsync(int gameId)
        {
            var game = await _context.Games
                .Include(g => g.Scores)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new Exception("Game not found");

            if (game.IsFinished)
                throw new Exception("Game already finished");

            if (!game.Scores.Any())
                throw new Exception("No scores registered");

            // Beregn totals pr spiller
            var totals = game.Scores
                .GroupBy(s => s.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Total = g.Sum(x => x.Points)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var winnerData = totals.First();

            //  Opret HallOfFame entry
            var hallOfFame = new HallOfFame
            {
                GameId = game.Id,
                UserId = winnerData.UserId,
                WinningScore = winnerData.Total,
                WinDate = DateTime.UtcNow
            };

            _context.HallOfFames.Add(hallOfFame);

            // Mark game as finished
            game.IsFinished = true;
            game.EndedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Return DTO (så vi undgår JSON cycle)
            return new GameDto
            {
                Id = game.Id,
                GameNumber = game.GameNumber,
                StartedAt = game.StartedAt,
                EndedAt = game.EndedAt,
                IsFinished = game.IsFinished,
            };
        }


        public async Task<List<GameDto>> GetAllGamesAsync()
        {
            var games = await _context.Games
                .Select(g => new GameDto
                {
                    Id = g.Id,
                    GameNumber = g.GameNumber,
                    StartedAt = g.StartedAt,
                    EndedAt = g.EndedAt,
                    IsFinished = g.IsFinished,

                    Participants = g.Participants.Select(p => new ParticipantDto
                    {
                        UserId = p.UserId,
                        UserName = p.User.Name
                    }).ToList(),

                    Scores = g.Scores.Select(s => new ScoreDto
                    {
                        Id= s.Id,
                        UserId = s.UserId,
                        UserName = s.User.Username,
                        Points = s.Points
                    }).ToList(),

                    Winner = g.Winner == null ? null : new WinnerDto
                    {
                        UserId = g.Winner.UserId,
                        UserName = g.Winner.User.Name,
                        WinningScore = g.Winner.WinningScore,
                        WinDate = g.Winner.WinDate
                    }
                })
                .ToListAsync();

            return games;
        }



        // Get a single game
        public async Task<GameDto?> GetGameByIdAsync(int id)
        {
            var game = await _context.Games
                .Include(g => g.Scores)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) return null;

            return new GameDto
            {
                Id = game.Id,
                GameNumber = game.GameNumber,
                StartedAt = game.StartedAt,
                EndedAt = game.EndedAt,
                IsFinished = game.IsFinished,
                Scores = game.Scores.Select(s => new ScoreDto
                {
                    UserId = s.UserId,
                    Points = s.Points
                }).ToList()
            };
        }


        public async Task AddParticipantsAsync(int gameId, List<int> userIds)
        {
            foreach (var userId in userIds)
            {
                var exists = await _context.GameParticipants
                    .AnyAsync(gp => gp.GameId == gameId && gp.UserId == userId);

                if (!exists)
                {
                    _context.GameParticipants.Add(new GameParticipant
                    {
                        GameId = gameId,
                        UserId = userId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        // Hent deltagere til et spil
        public async Task<List<ParticipantDto>> GetParticipantsAsync(int gameId)
        {
            return await _context.GameParticipants
                .Where(gp => gp.GameId == gameId)
                .Include(gp => gp.User)
                .Select(gp => new ParticipantDto
                {
                    UserId = gp.UserId,
                    UserName = gp.User.Name
                })
                .ToListAsync();
        }

        // Check om en bruger er deltager
        public async Task<bool> IsUserParticipantAsync(int gameId, int userId)
        {
            return await _context.GameParticipants
                .AnyAsync(gp => gp.GameId == gameId && gp.UserId == userId);
        }

    }
}
