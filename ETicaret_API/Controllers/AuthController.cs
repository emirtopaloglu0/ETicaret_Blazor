using ETicaret_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ETicaret_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _authService.RegisterAsync(request.Email, request.FirstName, request.LastName, request.Password);
        return Ok("Kayıt başarılı.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Email, request.Password);
        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _authService.GetLoggedUser(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role
        });
    }

    [Authorize(Roles = "admin")]
    [HttpPut("me/{id}")]
    public async Task<IActionResult> UpdateCurrentUser(int id, UpdateUserRequest request)
    {
        //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _authService.UpdateUserAsync
            (id, request.Email, request.FirstName, request.LastName, request.Password, request.role);

        return NoContent();
    }

}

public record RegisterRequest(string Email, string FirstName, string LastName, string Password);
public record LoginRequest(string Email, string Password);
public record UpdateUserRequest(string Email, string FirstName, string LastName, string Password, string role);
