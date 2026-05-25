namespace MekkysCakes.Shared.DTOs.ReviewDTOs
{
    public record ProductReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string UserDisplayName { get; set; } = null!;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
