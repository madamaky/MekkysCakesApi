namespace MekkysCakes.Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public OrderAddress Address { get; set; } = default!;

        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;

        public ICollection<OrderItem> Items { get; set; } = [];

        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
    }
}
