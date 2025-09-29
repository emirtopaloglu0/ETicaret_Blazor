using Azure.Core;
using ETicaret_Application.DTOs.UserDTOs;
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

    public async Task<LoggedUserDto> GetLoggedUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return new LoggedUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }
    public async Task<bool> UpdateUserAsync(int id, string email, string FirstName, string LastName, string password, string role = "customer")
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            user.Email = email;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.Role = role;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task ChangeUserRole(int id, string role, int companyId = 0, int shopId = 0)
    {
        var user = await _context.Users.FindAsync(id);
        if (user.Role == "shopUser")
        {
            var shopUser = await _context.ShopUsers.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (role != "shopUser")
            {
                _context.ShopUsers.Remove(shopUser);
            }
            else
            {
                if (shopId != 0)
                    shopUser.ShopId = shopId;
            }
        }
        else if (user.Role != "shopUser" && role == "shopUser")
        {
            Data.Entities.ShopUser shopUser = new();
            shopUser.UserId = user.Id;
            shopUser.ShopId = shopId;
            _context.ShopUsers.Add(shopUser);
        }

        if (user.Role == "deliverer")
        {
            var deliverer = await _context.Deliverers.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (role != "deliverer")
            {
                _context.Deliverers.Remove(deliverer);
            }
            else
            {
                if (companyId != 0)
                    deliverer.CompanyId = companyId;
            }
        }
        else if (user.Role != "deliverer" && role == "deliverer")
        {
            Deliverer deliverer = new();
            deliverer.UserId = user.Id;
            deliverer.CompanyId = companyId;
            _context.Deliverers.Add(deliverer);
        }

        user.Role = role;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckByMail(string mail)
    {
        return await _context.Users.AnyAsync(x => x.Email == mail);
    }

    public async Task<bool> IsPasswordRight(string password, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return false;
        return true;
    }

    public async Task<LoggedUserDto> GetByMail(string mail)
    {
        try
        {
            var response = await _context.Users.FirstOrDefaultAsync(x => x.Email == mail);
            LoggedUserDto loggedUser = new LoggedUserDto
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Email = response.Email,
                Role = response.Role,
            };
            return loggedUser;
        }
        catch
        {
            return null;
        }
    }
}
