namespace MekkysCakes.Shared.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public string ProductType { get; set; } = default!;
        public string ProductTheme { get; set; } = default!;
        public bool InStock { get; set; }
    }
}
