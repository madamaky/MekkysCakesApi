using MekkysCakes.Shared;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Services.Abstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(string email, OrderDTO orderDTO);
        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(string email, Guid id);
        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);
        Task<Result<PaginatedResult<OrderToReturnDTO>>> GetAllOrdersForAdminAsync(OrderQueryParams queryParams);
        Task<Result<bool>> CancelOrderAsync(string email, Guid orderId);
        Task<Result<bool>> UpdateOrderStatusAsync(Guid orderId, OrderStatusDTO newStatus);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods();

        //Task<Result<OrderStatsDTO>> GetOrderStatisticsAsync();
    }
}
