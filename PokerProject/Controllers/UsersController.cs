using Microsoft.AspNetCore.Mvc;
using PokerProject.DTOs;
using PokerProject.Models;
using PokerProject.Services;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }



    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto dto)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }


    [HttpGet]
    public async Task<ActionResult<UserDto>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    //[HttpPost("login")]
    //public async Task<ActionResult<UserDto>> Login(LoginUserDto dto)
    //{
    //    var user = await _userService.LoginAsync(dto.Username, dto.Password);

    //    if (user == null)
    //        return Unauthorized("Invalid username or password");

    //    return Ok(user);
    //}

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(LoginUserDto dto)
    {
        var token = await _userService.LoginAndGenerateTokenAsync(dto.Username, dto.Password);

        if (token == null)
            return Unauthorized("Invalid username or password");

        return Ok(new { Token = token });
    }


}
