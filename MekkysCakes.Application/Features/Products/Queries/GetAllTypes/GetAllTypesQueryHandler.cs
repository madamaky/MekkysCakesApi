using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;


namespace MekkysCakes.Application.Features.Products.Queries.GetAllTypes
{
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IEnumerable<TypeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TypeDTO>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
            => _mapper.Map<IEnumerable<TypeDTO>>(
                    await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync()
                );
    }
}
