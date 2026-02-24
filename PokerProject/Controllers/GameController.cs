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
        public async Task<ActionResult<GameDto>> StartGame()
        {
            var game = await _gameService.StartGameAsync();
            return Ok(game);
        }

        // Add game points
        [HttpPost("{gameId}/score")]
        public async Task<ActionResult<ScoreDto>> AddScore(int gameId, [FromBody] AddScoreDto dto)
        {
            try
            {
                var added = await _gameService.AddScoreAsync(gameId, dto.UserId, dto.Value);
                return Ok(added);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // End game
        [HttpPost("{gameId}/end")]
        public async Task<ActionResult<GameDto>> EndGame(int gameId)
        {
            try
            {
                var ended = await _gameService.EndGameAsync(gameId);
                return Ok(ended);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           
        }

        [HttpPost("{gameId}/cancel")]
        public async Task<ActionResult<GameDto>> CancelGame(int gameId)
        {
            try
            {
                var game = await _gameService.CancelGameAsync(gameId);
                return Ok(game);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Get all games
        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetAllGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDetailsDto>> GetGameDetails(int id)
        {
            try
            {
            var game = await _gameService.GetGameDetailsAsync(id);


            if (game == null)
                return NotFound();
            return Ok(game);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        // Tilføj deltagere
        [HttpPost("{gameId}/participants")]
        public async Task<IActionResult> AddParticipants(int gameId, [FromBody] AddParticipantsDto dto)
        {
            await _gameService.AddParticipantsAsync(gameId, dto.UserIds);
            return Ok();
        }

        // Hent deltagere
        [HttpGet("{gameId}/participants")]
        public async Task<List<ParticipantDto>> GetParticipants(int gameId)
        {
            return await _gameService.GetParticipantsAsync(gameId);
        }
    }
}
