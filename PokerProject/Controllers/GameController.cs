using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokerProject.DTOs;
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
                //I prefer getting roles this way.
                var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                var game = await _gameService.GetGameDetailsAsync(id, role);

                if (game == null)
                    return NotFound();

                return Ok(game);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("{gameId}/participants")]
        public async Task<IActionResult> AddParticipants(int gameId, [FromBody] AddParticipantsDto dto)
        {
            await _gameService.AddParticipantsAsync(gameId, dto.UserIds);
            return Ok();
        }

        [HttpGet("{gameId}/participants")]
        public async Task<List<ParticipantDto>> GetParticipants(int gameId)
        {
            return await _gameService.GetParticipantsAsync(gameId);
        }

        [HttpDelete("{gameId}/participants/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveParticipant(int gameId, int userId)
        {

            try
            {
                var updatedParticipants = await _gameService.RemoveParticipantAsync(gameId, userId);
                return Ok(updatedParticipants);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("{gameId}/players/{userId}/scores")]
        public async Task<ActionResult<PlayerScoreDetailsDto>> GetPlayerScores(int gameId, int userId)
        {
            try
            {
                var result = await _gameService.GetPlayerScoreEntries(gameId, userId);
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        // Controller
        [HttpDelete("points/{scoreId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveGamePoints(int scoreId)
        {
            try
            {
                var updatedScore = await _gameService.RemoveScoreAsync(scoreId);
                return Ok(updatedScore);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
