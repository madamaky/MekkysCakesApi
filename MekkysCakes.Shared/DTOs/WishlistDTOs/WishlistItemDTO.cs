using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Shared.DTOs.WishlistDTOs
{
    public class WishlistItemDTO
    {
        public DateTime DateAdded { get; set; }
        public ProductDTO Product { get; set; } = default!;
    }
}
