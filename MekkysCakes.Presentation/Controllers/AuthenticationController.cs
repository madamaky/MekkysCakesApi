using MediatR;
using MekkysCakes.Application.Features.Authentication.Commands.Login;
using MekkysCakes.Application.Features.Authentication.Commands.Register;
using MekkysCakes.Application.Features.Authentication.Queries.CheckEmail;
using MekkysCakes.Application.Features.Authentication.Queries.GetCurrentUser;
using MekkysCakes.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    /// <summary>
    /// Manages user authentication, registration, and user session info.
    /// </summary>
    public class AuthenticationController : ApiBaseController
    {
        private readonly ISender _sender;

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Authenticates a user and returns a generated JWT token.
        /// </summary>
        /// <param name="command">User login credentials.</param>
        /// <returns>A user data transfer object alongside the JWT token.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginCommand command)
        {
            var result = await _sender.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Registers a new user and returns a generated JWT token.
        /// </summary>
        /// <param name="command">Details of the user to be registered.</param>
        /// <returns>The created user and a newly generated JWT token.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterCommand command)
        {
            var result = await _sender.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Checks if an email address is already in use by an existing user.
        /// </summary>
        /// <param name="email">The email addresses to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("checkEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _sender.Send(new CheckEmailQuery(email));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the profile information of the currently authenticated user.
        /// </summary>
        /// <returns>The authenticated user's details.</returns>
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = GetEmailFromToken();
            var result = await _sender.Send(new GetCurrentUserQuery(email));
            return HandleResult(result);
        }
    }
}