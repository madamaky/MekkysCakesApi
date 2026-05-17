using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Shared.DTOs.ProductDTOs
{
    public record ProductDTO
    {
        public int Id { get; set; }
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Description { get; set; } = new();
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public LocalizedString ProductType { get; set; } = new();
        public LocalizedString ProductTheme { get; set; } = new();
        public bool InStock { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<LocalizedString> Badges { get; set; } = [];
    }
}
