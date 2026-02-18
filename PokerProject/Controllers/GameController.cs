using Microsoft.AspNetCore.Mvc;
using PokerProject.Services;
using PokerProject.Models;

namespace PokerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        // Start a new game
        [HttpPost("start")]
        public async Task<ActionResult<Game>> StartGame()
        {
            var game = await _gameService.StartGameAsync();
            return Ok(game);
        }

        // Add score
        [HttpPost("{gameId}/score")]
        public async Task<ActionResult<Score>> AddScore(int gameId, [FromBody] Score score)
        {
            var added = await _gameService.AddScoreAsync(gameId, score.UserId, score.Value);
            return Ok(added);
        }

        // End game
        [HttpPost("{gameId}/end")]
        public async Task<ActionResult<Game>> EndGame(int gameId)
        {
            var ended = await _gameService.EndGameAsync(gameId);
            return Ok(ended);
        }

        // Get all games
        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetAllGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        // Get single game
        [HttpGet("{gameId}")]
        public async Task<ActionResult<Game>> GetGame(int gameId)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);
            if (game == null) return NotFound();
            return Ok(game);
        }
    }
}
