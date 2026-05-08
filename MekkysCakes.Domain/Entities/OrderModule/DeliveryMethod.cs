namespace MekkysCakes.Domain.Entities.OrderModule
{
    public class DeliveryMethod : BaseEntity<int>, ITranslatableEntity<DeliveryMethodTranslation>
    {
        //public string ShortName { get; set; } = default!;
        //public string Description { get; set; } = default!;
        //public string DeliveryTime { get; set; } = default!;
        public decimal Price { get; set; }

        public ICollection<DeliveryMethodTranslation> Translations { get; set; } = [];
    }
}