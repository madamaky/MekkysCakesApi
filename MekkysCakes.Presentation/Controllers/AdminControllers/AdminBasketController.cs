using MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket;
using MekkysCakes.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers.AdminControllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminDashboard)]
    [Route("api/admin/baskets")]
    public class AdminBasketController : ApiBaseController
    {
        /// <summary> Delete basket </summary>
        /// <remarks> Deletes a user's shopping basket. </remarks>
        /// <response code="200">Basket was successfully deleted</response>
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var result = await Sender.Send(new DeleteBasketCommand(basketId));
            return HandleResult(result);
        }
    }
}
