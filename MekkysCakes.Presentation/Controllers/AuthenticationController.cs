using System.Security.Claims;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var result = await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("checkEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _authenticationService.CheckEmailAsync(email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var result = await _authenticationService.GetUserByEmailAsync(email);
            return HandleResult(result);
        }
    }
}