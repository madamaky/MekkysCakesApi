using MekkysCakes.Application.Features.Orders.Commands.UpdateOrderStatus;
using MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin;
using MekkysCakes.Domain;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers.AdminControllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminDashboard)]
    [Route("api/admin/orders")]
    public class AdminOrderController : ApiBaseController
    {
        /// <summary> Get all orders </summary>
        /// <remarks> Retrieves all orders in the system. </remarks>
        /// <response code="200">Returns a paginated list of all orders across the platform</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<OrderToReturnDTO>>> GetAllOrders([FromQuery] OrderQueryParams queryParams)
        {
            var result = await Sender.Send(new GetAllOrdersForAdminQuery(queryParams));
            return Ok(result);
        }

        /// <summary> Update order status </summary>
        /// <remarks> Updates the status of an existing order. </remarks>
        /// <response code="200">Returns true if the update was successful</response>
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<bool>> UpdateOrderStatus(Guid orderId, OrderStatusDTO newStatus)
        {
            var result = await Sender.Send(new UpdateOrderStatusCommand(orderId, newStatus));
            return HandleResult(result);
        }
    }
}
