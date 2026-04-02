namespace MekkysCakes.Domain.Entities.OrderModule
{
    public class OrderItem : BaseEntity<int>
    {
        public ProductItemOrdered Product { get; set; } = default!;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? CustomMessage { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
    }
}
