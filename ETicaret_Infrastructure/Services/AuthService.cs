using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using ETicaret_Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ETicaret_Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ETicaretDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ETicaretDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task RegisterAsync(string email, string FirstName, string LastName, string password, string role = "customer")
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
            throw new Exception("Bu email zaten kayıtlı.");

        var user = new ETicaret_Infrastructure.Data.Entities.User
        {
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            FirstName = FirstName,
            LastName = LastName
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            throw new UnauthorizedAccessException("Email veya şifre yanlış.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
