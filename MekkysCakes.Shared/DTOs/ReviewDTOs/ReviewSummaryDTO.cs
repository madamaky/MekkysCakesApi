namespace MekkysCakes.Shared.DTOs.ReviewDTOs
{
    public class ReviewSummaryDTO
    {
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
    }
}
