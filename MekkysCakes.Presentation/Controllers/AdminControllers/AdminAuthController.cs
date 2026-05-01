using MekkysCakes.Application.Features.Authentication.Queries.CheckEmail;
using MekkysCakes.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers.AdminControllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminDashboard)]
    [Route("api/admin/auth")]
    public class AdminAuthController : ApiBaseController
    {
        /// <summary> Check email </summary>
        /// <remarks> Checks if an email address is already in use by an existing user. </remarks>
        /// <response code="200">Returns boolean indicating if email is taken</response>
        [HttpGet("check-email")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await Sender.Send(new CheckEmailQuery(email));
            return Ok(result);
        }
    }
}
