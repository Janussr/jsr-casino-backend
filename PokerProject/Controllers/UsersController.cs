using Microsoft.AspNetCore.Mvc;
using PokerProject.Data;
using PokerProject.DTOs;
using PokerProject.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly PokerDbContext _context;

    public UsersController(PokerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Name = u.Name
            })
            .ToListAsync();
    }
}
