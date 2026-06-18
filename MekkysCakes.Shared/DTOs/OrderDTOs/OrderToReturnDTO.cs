namespace MekkysCakes.Shared.DTOs.OrderDTOs
{
    public record OrderToReturnDTO
    {
        public Guid Id { get; init; }
        public string UserName { get; set; } = default!;
        public string ContactEmail { get; init; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; init; } = default!;
        public string OrdersStatus { get; init; } = default!;
        public DateTimeOffset OrderDate { get; init; }
        public LocalizedString DeliveryMethodName { get; init; } = default!;
        public ICollection<OrderItemDTO> Items { get; init; } = new List<OrderItemDTO>();
        public decimal Subtotal { get; init; }
        public decimal Total { get; init; }
    }
}
