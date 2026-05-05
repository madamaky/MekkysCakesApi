using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllBadges
{
    public record GetAllBadgesQueryHandler : IRequestHandler<GetAllBadgesQuery, IEnumerable<BadgeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBadgesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BadgeDTO>> Handle(GetAllBadgesQuery request, CancellationToken cancellationToken)
            => _mapper.Map<IEnumerable<BadgeDTO>>(
                    await _unitOfWork.GetRepository<Badge, int>().GetAllAsync()
                );
    }
}
