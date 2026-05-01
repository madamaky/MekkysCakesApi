using MekkysCakes.Application.Features.Orders.Commands.CancelOrder;
using MekkysCakes.Application.Features.Orders.Commands.CreateOrder;
using MekkysCakes.Application.Features.Orders.Queries.GetAllOrders;
using MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods;
using MekkysCakes.Application.Features.Orders.Queries.GetOrderById;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        /// <summary> Create order </summary>
        /// <remarks> Converts the current basket into an active order for the currently authenticated user. </remarks>
        /// <response code="200">Returns the created order information</response>
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(CreateOrderCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResult(result);
        }

        /// <summary> Get order </summary>
        /// <remarks> Retrieves full details of a specific order belonging to the currently authenticated user. </remarks>
        /// <response code="200">Returns the detailed information of the specific order</response>
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var result = await Sender.Send(new GetOrderByIdQuery(id));
            return HandleResult(result);
        }

        /// <summary> Get orders </summary>
        /// <remarks> Retrieves all orders placed by the currently authenticated user. </remarks>
        /// <response code="200">Returns a collection of the user's orders</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var result = await Sender.Send(new GetAllOrdersQuery());
            return HandleResult(result);
        }

        /// <summary> Cancel order </summary>
        /// <remarks> Cancels a specific order if it is still eligible for cancellation. </remarks>
        /// <response code="200">True if manually cancelled successfully</response>
        [Authorize]
        [HttpPut("CancelOrder")]
        public async Task<ActionResult<bool>> CancelOrder(Guid orderId)
        {
            var result = await Sender.Send(new CancelOrderCommand(orderId));
            return HandleResult(result);
        }

        /// <summary> Get delivery methods </summary>
        /// <remarks> Retrieves a list of available delivery methods. </remarks>
        /// <response code="200">Returns a collection of valid delivery methods with their pricing details</response>
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await Sender.Send(new GetDeliveryMethodsQuery());
            return HandleResult(result);
        }
    }
}
