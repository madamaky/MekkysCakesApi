using MediatR;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ReviewDTOs;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews
{
    public record GetAllReviewsQuery(ReviewQueryParams queryParams) : IRequest<PaginatedResult<ReviewDTO>>;

    public class ReviewQueryParams : PaginatedQueryParams
    {
        public int? ProductId { get; set; }
        public bool? IsApproved { get; set; }
        public ReviewSortingOptions Sort { get; set; }

        protected override int DefaultPageSize => 10;
        protected override int MaxPageSize => 20;
    }

    public record ReviewDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductPictureUrl { get; set; }
        public string UserId { get; set; } = default!;
        public string UserDisplayName { get; set; } = default!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
