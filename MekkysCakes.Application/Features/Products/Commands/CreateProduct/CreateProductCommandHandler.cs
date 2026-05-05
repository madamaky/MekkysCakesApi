using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<bool>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var type = await _unitOfWork.GetRepository<ProductType, int>().GetByIdAsync(request.TypeId);
            if (type is null)
                return Error.NotFound("ProductType.NotFound", $"The Product Type With Id {request.TypeId} Was Not Found");

            var theme = await _unitOfWork.GetRepository<ProductTheme, int>().GetByIdAsync(request.ThemeId);
            if (theme is null)
                return Error.NotFound("ProductTheme.NotFound", $"The Product Theme With Id {request.ThemeId} Was Not Found");

            // Validate all badge ids exist
            var badgeRepo = _unitOfWork.GetRepository<Badge, int>();
            foreach (var badgeId in request.BadgeIds.Distinct())
            {
                var badge = await badgeRepo.GetByIdAsync(badgeId);
                if (badge is null)
                    return Error.NotFound("Badge.NotFound", $"The Badge With Id {badgeId} Was Not Found");
            }

            var product = _mapper.Map<Product>(request);

            // Add badge associations
            product.ProductBadges = request.BadgeIds.Distinct()
                .Select(id => new ProductBadge { BadgeId = id })
                .ToList();

            await _unitOfWork.GetRepository<Product, int>().AddAsync(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
