using ETicaret_Application.Services;
using System.Security.Claims;

namespace ETicaret_API
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _ctx;
        public CurrentUserService(IHttpContextAccessor ctx) => _ctx = ctx;
        public int? UserId => int.TryParse(_ctx.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : (int?)null;
        public string? Role => _ctx.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }

}
