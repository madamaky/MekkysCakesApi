namespace MekkysCakes.Shared.DTOs.WishlistDTOs
{
    public class WishlistDTO
    {
        public string UserEmail { get; set; } = default!;
        public ICollection<WishlistItemDTO> Items { get; set; } = new List<WishlistItemDTO>();
    }
}
