using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.DTOs.UserDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_UI.Enums;
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
                user.Role,
                user.Address,
                user.PhoneNumber
            });
        }

        //[Authorize(Roles = $"{UserRoleEnums.Admin}")]
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

        [Authorize]
        [HttpGet("IsPasswordRight/{password}/{userId}")]
        public async Task<bool> IsUserPasswordRight(string password, int userId)
        {
            var response = await _authService.IsPasswordRight(password, userId);
            return response;
        }

        [HttpGet("GetByMail/{mail}")]
        public async Task<ActionResult<IEnumerable<LoggedUserDto>>> GetUserByMail(string mail)
        {
            var response = await _authService.GetByMail(mail);
            if (response is null) { return NotFound("Bulunamadı"); }
            return Ok(response);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _authService.UpdateUserAsync
                (userId, request.Email, request.FirstName, request.LastName, request.Password, request.Address, request.PhoneNumber);

            return Ok(user);
        }

        [Authorize(Roles = UserRoleEnums.Admin)]
        [HttpPut("changeRole/{id}")]
        public async Task<IActionResult> ChangeRole(int id, [FromBody] ChangeRole changeRole)
        {
            await _authService.ChangeUserRole(id, changeRole.role, changeRole.companyId, changeRole.shopId);
            return Ok();
        }
    }
    public record UpdateUserRequest(string Email, string FirstName, string LastName, string Password, string Address, string PhoneNumber);
    public record ChangeRole(string role, int companyId = 0, int shopId = 0);

}
