using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using MekkysCakes.Application.Helpers;

namespace MekkysCakes.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILanguageContext _languageContext;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILanguageContext languageContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _languageContext = languageContext;
        }

        public async Task<Result<ProductDTO>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductWithTypeAndThemeSpecification(request.Id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {request.Id} Was Not Found");

            //return _mapper.Map<ProductDTO>(product);

            var lang = _languageContext.CurrentLanguage;
            return MapProductToDTO(product, lang);
        }

        private ProductDTO MapProductToDTO(Product product, string lang)
        {
            var t = TranslationHelper.GetTranslation(product, lang);
            var themeT = TranslationHelper.GetTranslation(product.ProductTheme, lang);
            var typeT = TranslationHelper.GetTranslation(product.ProductType, lang);

            return new ProductDTO
            {
                Id = product.Id,
                Name = t?.Name ?? "",
                Description = t?.Description ?? "",
                PictureUrl = product.PictureUrl, // resolve URL separately
                Price = product.Price,
                ProductTheme = themeT?.Name ?? "",
                ProductType = typeT?.Name ?? "",
                InStock = product.InStock,
                AverageRating = product.AverageRating,
                TotalReviews = product.TotalReviews,
                Badges = product.ProductBadges
                    .Select(pb => TranslationHelper.GetTranslation(pb.Badge, lang)?.Name ?? "")
                    .ToList()
            };
        }
    }
}
