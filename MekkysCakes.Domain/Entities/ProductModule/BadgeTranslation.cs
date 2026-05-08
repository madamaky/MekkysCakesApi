namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class BadgeTranslation : BaseEntity<int>, ITranslation
    {
        public string Language { get; set; }= default!;
        public string Name { get; set; } = default!;

        public int BadgeId { get; set; }
        public Badge Badge { get; set; } = default!;
    }
}