using MekkysCakes.Application.Features.Authentication.Commands.Login;
using MekkysCakes.Application.Features.Authentication.Commands.Register;
using MekkysCakes.Application.Features.Authentication.Queries.GetCurrentUser;
using MekkysCakes.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        /// <summary> Login </summary>
        /// <remarks> Authenticates a user and returns a generated JWT token. </remarks>
        /// <response code="200">Returns the user details and JWT token</response>
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResult(result);
        }

        /// <summary> Register </summary>
        /// <remarks> Registers a new user and returns a generated JWT token. </remarks>
        /// <response code="200">Successfully registered and returns user profile</response>
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResult(result);
        }

        /// <summary> Get current user </summary>
        /// <remarks> Retrieves the profile information of the currently authenticated user. </remarks>
        /// <response code="200">Returns the currently authenticated user</response>
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var result = await Sender.Send(new GetCurrentUserQuery());
            return HandleResult(result);
        }
    }
}