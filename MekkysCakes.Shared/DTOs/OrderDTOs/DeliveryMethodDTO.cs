namespace MekkysCakes.Shared.DTOs.OrderDTOs
{
    public class DeliveryMethodDTO
    {
        public int Id { get; set; }
        public string ShortName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DeliveryTime { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
