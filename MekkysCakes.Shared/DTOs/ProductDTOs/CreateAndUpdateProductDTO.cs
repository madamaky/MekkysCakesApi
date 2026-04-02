using System.ComponentModel.DataAnnotations;

namespace MekkysCakes.Shared.DTOs.ProductDTOs
{
    public class CreateAndUpdateProductDTO
    {
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Type Id must be a non-negative integer.")]
        public int TypeId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Theme Id must be a non-negative integer.")]
        public int ThemeId { get; set; }
    }
}
