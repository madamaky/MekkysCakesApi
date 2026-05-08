namespace MekkysCakes.Domain.Entities.OrderModule
{
    public class DeliveryMethodTranslation : BaseEntity<int>, ITranslation
    {
        public string Language { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string DeliveryTime { get; set; } = default!;

        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
    }
}