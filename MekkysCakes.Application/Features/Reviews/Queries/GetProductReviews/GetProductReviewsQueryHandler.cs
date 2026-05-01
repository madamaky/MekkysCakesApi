using AutoMapper;
using MediatR;
using MekkysCakes.Application.Specifications.ReviewSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared;


namespace MekkysCakes.Application.Features.Reviews.Queries.GetProductReviews
{
    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, PaginatedResult<ReviewDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ReviewDTO>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ProductReview, int>();

            // Fetch the paginated, sorted, apporved reviews (with user included)
            var spec = new ProductReviewSpecification(request.ProductId, request.Sort, request.PageSize, request.PageIndex);
            var reviews = await repo.GetAllAsync(spec);

            // Get total count of approved reviews (for pagination)
            var countSpec = new ReviewCountSpecification(request.ProductId);
            var totalCount = await repo.CountAsync(countSpec);

            // Map to DTOs
            var reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);

            // Return paginated result
            return new PaginatedResult<ReviewDTO>
            (
                request.PageIndex,
                reviewDTOs.Count(),
                totalCount,
                reviewDTOs
            );
        }
    }
}
