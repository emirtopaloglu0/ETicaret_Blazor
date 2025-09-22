using ETicaret_Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
}

public record RegisterRequest(string Email, string FirstName, string LastName, string Password);
public record LoginRequest(string Email, string Password);
