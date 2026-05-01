using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MekkysCakes.Services.Abstraction;
using Microsoft.AspNetCore.Http;

namespace MekkysCakes.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Email  => _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Email);
        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        public string? Token => _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()["Bearer ".Length..];
    }
}
