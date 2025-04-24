using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Services;
using LibraryAPI.Data;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public ActionResult<string> Login([FromBody] User login)
    {
        var user = _context.Users.FirstOrDefault(u =>
            u.Username == login.Username && u.Password == login.Password); // ⚠️ Use hashing in real apps!

        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _tokenService.CreateToken(user);
        return Ok(token);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User newUser)
    {
        _context.Users.Add(newUser);
        _context.SaveChanges();
        return Ok("User registered successfully");
    }
}
