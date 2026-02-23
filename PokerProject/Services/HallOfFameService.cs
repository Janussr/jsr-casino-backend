using Microsoft.EntityFrameworkCore;
using PokerProject.Data;
using PokerProject.DTOs;

namespace PokerProject.Services
{

    public interface IHallOfFameService
    {
        Task<List<HallOfFameDto>> GetEntireHallOfFameAsync();
    }

    public class HallOfFameService : IHallOfFameService
    {
        private readonly PokerDbContext _context;
        public  HallOfFameService(PokerDbContext context) {

            _context = context;

        }
        public async Task<List<HallOfFameDto>> GetEntireHallOfFameAsync()
        {

            var hallOfFame = await _context.HallOfFames.Select(h => new HallOfFameDto
            {
                PlayerName = h.User.Name,
                //Wins = h.Wins,
            }).ToListAsync();

            return hallOfFame;
        }


    }
}
