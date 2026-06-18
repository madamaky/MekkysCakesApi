using MekkysCakes.Domain.Entities.IdentityModule;

namespace MekkysCakes.Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserName { get; set; } = default!;
        public string ContactEmail { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;

        public ICollection<OrderItem> Items { get; set; } = [];

        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
    }
}
