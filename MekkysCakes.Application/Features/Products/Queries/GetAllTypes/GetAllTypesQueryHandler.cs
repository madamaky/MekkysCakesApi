using AutoMapper;
using MediatR;
using MekkysCakes.Application.Features.Products.Queries.GetAllBadges;
using MekkysCakes.Application.Features.Products.Queries.GetAllThemes;
using MekkysCakes.Application.Helpers;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;


namespace MekkysCakes.Application.Features.Products.Queries.GetAllTypes
{
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IEnumerable<TypeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILanguageContext _languageContext;

        public GetAllTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILanguageContext languageContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _languageContext = languageContext;
        }

        public async Task<IEnumerable<TypeDTO>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            //return _mapper.Map<IEnumerable<TypeDTO>>(
            //        await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync()
            //    );

            var spec = new TypeWithTranslationsSpecification();
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(spec);
            var lang = _languageContext.CurrentLanguage;

            return types.Select(t =>
            {
                var translation = TranslationHelper.GetTranslation(t, lang);
                return new TypeDTO(t.Id, translation?.Name ?? "");
            });
        }
    }
}
