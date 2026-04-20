using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResult<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var specification = new ProductWithTypeAndThemeSpecification(request.QueryParams);
            var products = await repo.GetAllAsync(specification);
            var dataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var countOfAllProducts = await repo.CountAsync(new ProductsCountSpecification(request.QueryParams));
            return new PaginatedResult<ProductDTO>
            (
                request.QueryParams.PageIndex,
                dataToReturn.Count(),
                countOfAllProducts,
                dataToReturn
            );
        }
    }
}
