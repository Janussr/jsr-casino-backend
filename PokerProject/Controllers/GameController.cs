using Microsoft.AspNetCore.Mvc;
using PokerProject.DTOs;
using PokerProject.Models;
using PokerProject.Services;

namespace PokerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
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
        public async Task<ActionResult<Score>> AddScore(int gameId, [FromBody] AddScoreDto dto)
        {
            var added = await _gameService.AddScoreAsync(gameId, dto.UserId, dto.Value);
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
        public async Task<ActionResult<GameDto>> GetGame(int gameId)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);
            if (game == null) return NotFound();
            return Ok(game);
        }
    }
}
