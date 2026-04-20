using MediatR;
using MekkysCakes.Application.Features.Orders.Commands.CancelOrder;
using MekkysCakes.Application.Features.Orders.Commands.CreateOrder;
using MekkysCakes.Application.Features.Orders.Commands.UpdateOrderStatus;
using MekkysCakes.Application.Features.Orders.Queries.GetAllOrders;
using MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin;
using MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods;
using MekkysCakes.Application.Features.Orders.Queries.GetOrderById;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    /// <summary>
    /// Manages user orders, delivery methods, and administration of orders.
    /// </summary>
    public class OrderController : ApiBaseController
    {
        private readonly ISender _sender;

        public OrderController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Creates a new order for the currently authenticated user.
        /// </summary>
        /// <param name="command">Order details like basket ID and delivery address.</param>
        /// <returns>The created order information.</returns>
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(CreateOrderCommand command)
        {
            // Inject the user email from the JWT token into the command
            var commandWithEmail = command with { Email = GetEmailFromToken() };
            var result = await _sender.Send(commandWithEmail);
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves a specific order belonging to the currently authenticated user.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The detailed information of the specific order.</returns>
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var result = await _sender.Send(new GetOrderByIdQuery(GetEmailFromToken(), id));
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves all orders placed by the currently authenticated user.
        /// </summary>
        /// <returns>A collection of the user's orders.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var result = await _sender.Send(new GetAllOrdersQuery(GetEmailFromToken()));
            return HandleResult(result);
        }

        /// <summary>
        /// Cancels a specific order if it is still eligible for cancellation.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <returns>True if manually cancelled successfully; otherwise, false.</returns>
        [Authorize]
        [HttpPut("CancelOrder")]
        public async Task<ActionResult<bool>> CancelOrder(Guid orderId)
        {
            var result = await _sender.Send(new CancelOrderCommand(GetEmailFromToken(), orderId));
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves a list of available delivery methods.
        /// </summary>
        /// <returns>A collection of valid delivery methods with their pricing details.</returns>
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _sender.Send(new GetDeliveryMethodsQuery());
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves all orders in the system. Only accessible by administrators.
        /// </summary>
        /// <param name="queryParams">Filtering and pagination parameters.</param>
        /// <returns>A paginated list of all orders across the platform.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAllOrdersForAdmin")]
        public async Task<ActionResult<PaginatedResult<OrderToReturnDTO>>> GetOrdersForAdmin([FromQuery] OrderQueryParams queryParams)
        {
            var result = await _sender.Send(new GetAllOrdersForAdminQuery(queryParams));
            return Ok(result);
        }

        /// <summary>
        /// Updates the status of an existing order. Only accessible by administrators.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="newStatus">The new status information to apply.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("UpdateStatus/{orderId}")]
        public async Task<ActionResult<bool>> UpdateOrderStatus(Guid orderId, OrderStatusDTO newStatus)
        {
            var result = await _sender.Send(new UpdateOrderStatusCommand(orderId, newStatus));
            return HandleResult(result);
        }
    }
}
