using PokerProject.Data;
using PokerProject.DTOs;
using PokerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace PokerProject.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> RegisterAsync(RegisterUserDto dto);

    }

    public class UserService : IUserService
    {
        private readonly PokerDbContext _context;

        public UserService(PokerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users.Select(u => new UserDto {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name
                }).ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name
            };
        }


        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Name = dto.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name
            };
        }


    }
}
