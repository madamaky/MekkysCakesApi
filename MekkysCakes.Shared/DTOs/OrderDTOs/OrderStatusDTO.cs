namespace MekkysCakes.Shared.DTOs.OrderDTOs
{
    public enum OrderStatusDTO
    {
        Pending = 0,
        Confirmed = 1,
        Preparing = 2,
        OutForDelivery = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }
}
