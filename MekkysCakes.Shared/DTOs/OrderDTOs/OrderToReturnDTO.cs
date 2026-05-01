namespace MekkysCakes.Shared.DTOs.OrderDTOs
{
    public record OrderToReturnDTO
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = default!;
        public ICollection<OrderItemDTO> Items { get; init; } = new List<OrderItemDTO>();
        public AddressDTO Address { get; init; } = default!;
        public string DeliveryMethod { get; init; } = default!;
        public string OrdersStatus { get; init; } = default!;
        public DateTimeOffset OrderDate { get; init; }
        public decimal Subtotal { get; init; }
        public decimal Total { get; init; }
    }
}
