using MediatR;
using MekkysCakes.Application.Features.Authentication.Commands.Login;
using MekkysCakes.Application.Features.Authentication.Commands.Register;
using MekkysCakes.Application.Features.Authentication.Queries.CheckEmail;
using MekkysCakes.Application.Features.Authentication.Queries.GetCurrentUser;
using MekkysCakes.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
        /// <remarks>
        /// Validates the provided credentials and issues a JWT token.
        /// 
        /// Sample request:
        ///
        ///     POST /api/authentication/login
        ///     {
        ///        "email": "user@example.com",
        ///        "password": "Password123!"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the user details and JWT token</response>
        /// <response code="400">Invalid login credentials supplied</response>
        /// <response code="404">User not found</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> Login(LoginCommand command)
        {
            var result = await _sender.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Registers a new user and returns a generated JWT token.
        /// </summary>
        /// <param name="command">Details of the user to be registered.</param>
        /// <remarks>
        /// Creates a new user profile and returns a ready-to-use JWT token.
        /// 
        /// Sample request:
        ///
        ///     POST /api/authentication/register
        ///     {
        ///        "displayName": "John Doe",
        ///        "email": "johndoe@example.com",
        ///        "password": "Password123!"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully registered and returns user profile</response>
        /// <response code="400">Validation error during registration</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register(RegisterCommand command)
        {
            var result = await _sender.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Checks if an email address is already in use by an existing user.
        /// </summary>
        /// <param name="email">The email addresses to check.</param>
        /// <remarks>
        /// Useful for frontend validation during the registration process.
        /// 
        /// Sample request:
        ///
        ///     GET /api/authentication/checkEmail?email=test@xyz.com
        ///
        /// </remarks>
        /// <response code="200">Returns boolean indicating if email is taken</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("checkEmail")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _sender.Send(new CheckEmailQuery(email));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the profile information of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Uses the Bearer token to identify the user and retrieve their information.
        /// 
        /// Sample request:
        ///
        ///     GET /api/authentication/currentUser
        ///
        /// </remarks>
        /// <response code="200">Returns the currently authenticated user</response>
        /// <response code="401">Unauthorized if the token is missing or invalid</response>
        /// <response code="404">User associated with the token was not found</response>
        [Authorize]
        [HttpGet("currentUser")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = GetEmailFromToken();
            var result = await _sender.Send(new GetCurrentUserQuery(email));
            return HandleResult(result);
        }
    }
}