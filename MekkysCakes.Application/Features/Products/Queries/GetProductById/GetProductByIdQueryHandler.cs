using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductDTO>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductWithTypeAndThemeSpecification(request.Id);

            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specification);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {request.Id} Was Not Found");

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
