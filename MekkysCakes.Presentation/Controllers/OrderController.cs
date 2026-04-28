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
using Microsoft.AspNetCore.Http;

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
        /// <remarks>
        /// Converts the current basket into an active order.
        /// 
        /// Sample request:
        ///
        ///     POST /api/order/Create
        ///     {
        ///        "basketId": "basket-123",
        ///        "deliveryMethodId": 2,
        ///        "shippingAddress": {
        ///           "firstName": "John",
        ///           "lastName": "Doe",
        ///           "street": "123 Main St",
        ///           "city": "Metropolis",
        ///           "state": "NY",
        ///           "zipCode": "10001"
        ///        }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the created order information</response>
        /// <response code="400">Invalid order details or empty basket</response>
        /// <response code="401">Unauthorized access</response>
        [Authorize]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <remarks>
        /// Gets full order details including line items, shipping address, and total amount.
        /// 
        /// Sample request:
        ///
        ///     GET /api/order/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// </remarks>
        /// <response code="200">Returns the detailed information of the specific order</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Order isolated or not found for this user</response>
        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var result = await _sender.Send(new GetOrderByIdQuery(GetEmailFromToken(), id));
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves all orders placed by the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Lists historical and active orders associated with the user.
        /// 
        /// Sample request:
        ///
        ///     GET /api/order
        ///
        /// </remarks>
        /// <response code="200">Returns a collection of the user's orders</response>
        /// <response code="401">Unauthorized access</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderToReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var result = await _sender.Send(new GetAllOrdersQuery(GetEmailFromToken()));
            return HandleResult(result);
        }

        /// <summary>
        /// Cancels a specific order if it is still eligible for cancellation.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <remarks>
        /// Allows a user to cancel an order that hasn't been fulfilled yet.
        /// 
        /// Sample request:
        ///
        ///     PUT /api/order/CancelOrder?orderId=3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// </remarks>
        /// <response code="200">True if manually cancelled successfully</response>
        /// <response code="400">Order cannot be cancelled in its current state</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Order not found</response>
        [Authorize]
        [HttpPut("CancelOrder")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> CancelOrder(Guid orderId)
        {
            var result = await _sender.Send(new CancelOrderCommand(GetEmailFromToken(), orderId));
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves a list of available delivery methods.
        /// </summary>
        /// <remarks>
        /// Used by the frontend checkout process to display shipping options and prices.
        /// 
        /// Sample request:
        ///
        ///     GET /api/order/DeliveryMethods
        ///
        /// </remarks>
        /// <response code="200">Returns a collection of valid delivery methods with their pricing details</response>
        [HttpGet("DeliveryMethods")]
        [ProducesResponseType(typeof(IEnumerable<DeliveryMethodDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _sender.Send(new GetDeliveryMethodsQuery());
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves all orders in the system. Only accessible by administrators.
        /// </summary>
        /// <param name="queryParams">Filtering and pagination parameters.</param>
        /// <remarks>
        /// Exposes all customer orders for administrative viewing.
        /// 
        /// Sample request:
        ///
        ///     GET /api/order/GetAllOrdersForAdmin?pageIndex=1&amp;pageSize=10
        ///
        /// </remarks>
        /// <response code="200">Returns a paginated list of all orders across the platform</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAllOrdersForAdmin")]
        [ProducesResponseType(typeof(PaginatedResult<OrderToReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        /// <remarks>
        /// Handles business logic progressing an order through Fulfillment.
        /// 
        /// Sample request:
        ///
        ///     PUT /api/order/UpdateStatus/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///        "status": "Shipped"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns true if the update was successful</response>
        /// <response code="400">Invalid status update</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        /// <response code="404">Order not found</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("UpdateStatus/{orderId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateOrderStatus(Guid orderId, OrderStatusDTO newStatus)
        {
            var result = await _sender.Send(new UpdateOrderStatusCommand(orderId, newStatus));
            return HandleResult(result);
        }
    }
}
