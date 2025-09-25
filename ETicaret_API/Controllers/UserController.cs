using ETicaret_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETicaret_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = _authService.GetLoggedUser(id);
            return Ok(response);
        }

        [HttpGet("CheckByMail/{mail}")]
        public async Task<bool> CheckUserByMail(string mail)
        {
            var response = await _authService.CheckByMail(mail);
            return response;
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser(UpdateUserRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _authService.UpdateUserAsync
                (userId, request.Email, request.FirstName, request.LastName, request.Password);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("changeRole/{id}")]
        public async Task<IActionResult> ChangeRole(int id, string role)
        {
            await _authService.ChangeUserRole(id, role);
            return Ok();
        }
    }
    public record UpdateUserRequest(string Email, string FirstName, string LastName, string Password);

}
