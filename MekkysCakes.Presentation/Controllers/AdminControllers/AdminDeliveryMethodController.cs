using MekkysCakes.Application.Features.Orders.Commands.CreateDeliveryMethod;
using MekkysCakes.Application.Features.Orders.Commands.DeleteDeliveryMethod;
using MekkysCakes.Application.Features.Orders.Commands.UpdateDeliveryMethod;
using MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods;
using MekkysCakes.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers.AdminControllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminDashboard)]
    [Route("api/admin/delivery-methods")]
    public class AdminDeliveryMethodController : ApiBaseController
    {
        /// <summary> Get all delivery methods </summary>
        /// <remarks> Retrieves a list of all delivery methods with their translations. </remarks>
        /// <response code="200">Returns a collection of all delivery methods</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethods()
        {
            var result = await Sender.Send(new GetDeliveryMethodsQuery());
            return HandleResult(result);
        }

        /// <summary> Create delivery method </summary>
        /// <remarks> Creates a new delivery method. </remarks>
        /// <response code="200">Returns true if the delivery method was successfully created</response>
        [HttpPost]
        public async Task<ActionResult<bool>> CreateDeliveryMethod(CreateDeliveryMethodCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResult(result);
        }

        /// <summary> Update delivery method </summary>
        /// <remarks> Updates an existing delivery method's details. </remarks>
        /// <response code="200">Returns true if the delivery method was successfully updated</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateDeliveryMethod([FromRoute] int id, [FromBody] UpdateDeliveryMethodCommand command)
        {
            var commandWithId = command with { Id = id };
            var result = await Sender.Send(commandWithId);
            return HandleResult(result);
        }

        /// <summary> Delete delivery method </summary>
        /// <remarks> Deletes a delivery method. </remarks>
        /// <response code="200">Returns true if the delivery method was successfully deleted</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteDeliveryMethod(int id)
        {
            var result = await Sender.Send(new DeleteDeliveryMethodCommand(id));
            return HandleResult(result);
        }
    }
}
