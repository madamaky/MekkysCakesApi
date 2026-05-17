using AutoMapper;
using MediatR;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllBadges
{
    public class GetAllBadgesQueryHandler : IRequestHandler<GetAllBadgesQuery, IEnumerable<BadgeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBadgesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BadgeDTO>> Handle(GetAllBadgesQuery request, CancellationToken cancellationToken)
        {
            var spec = new BadgeWithTranslationsSpecification();
            var badges = await _unitOfWork.GetRepository<Badge, int>().GetAllAsync(spec);

            return _mapper.Map<IEnumerable<BadgeDTO>>(badges);
        }
    }
}
