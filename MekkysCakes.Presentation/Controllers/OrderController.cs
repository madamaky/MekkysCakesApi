using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Result = await _orderService.CreateOrderAsync(GetEmailFromToken(), orderDTO);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var Result = await _orderService.GetOrderByIdAsync(GetEmailFromToken(), id);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var Result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
            return HandleResult(Result);
        }

        [Authorize]
        [HttpPut("CancelOrder")]
        public async Task<ActionResult<bool>> CancelOrder(Guid orderId)
        {
            var Result = await _orderService.CancelOrderAsync(GetEmailFromToken(), orderId);
            return HandleResult(Result);
        }


        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var Result = await _orderService.GetDeliveryMethods();
            return HandleResult(Result);
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAllOrdersForAdmin")]
        public async Task<ActionResult<PaginatedResult<OrderToReturnDTO>>> GetOrdersForAdmin([FromQuery] OrderQueryParams queryParams)
        {
            var result = await _orderService.GetAllOrdersForAdminAsync(queryParams);
            return HandleResult(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("UpdateStatus/{orderId}")]
        public async Task<ActionResult<bool>> UpdateOrderStatus(Guid orderId, OrderStatusDTO newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return HandleResult(result);
        }
    }
}
