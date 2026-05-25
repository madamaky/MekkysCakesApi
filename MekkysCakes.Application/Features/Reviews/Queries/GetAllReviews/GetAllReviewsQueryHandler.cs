using AutoMapper;
using MediatR;
using MekkysCakes.Application.Specifications.ReviewSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ReviewDTOs;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews
{
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, PaginatedResult<ReviewDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ReviewDTO>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<ProductReview, int>();

            // Get all reviews for the product with filteraion, pagination, and sorting
            var spec = new ProductReviewSpecification(request.queryParams);
            var reviews = await repo.GetAllAsync(spec);

            // Get total count of filtered reviews
            var countSpec = new ReviewCountSpecification(request.queryParams);
            var totalCount = await repo.CountAsync(countSpec);

            // Map to DTOs
            var reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);

            // Return paginated result
            return new PaginatedResult<ReviewDTO>
            (
                request.queryParams.PageIndex,
                reviewDTOs.Count(),
                totalCount,
                reviewDTOs
            );
        }
    }
}
