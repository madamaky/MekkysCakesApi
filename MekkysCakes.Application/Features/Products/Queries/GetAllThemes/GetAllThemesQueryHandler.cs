using AutoMapper;
using MediatR;
using MekkysCakes.Application.Features.Products.Queries.GetAllBadges;
using MekkysCakes.Application.Helpers;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;


namespace MekkysCakes.Application.Features.Products.Queries.GetAllThemes
{
    public class GetAllThemesQueryHandler : IRequestHandler<GetAllThemesQuery, IEnumerable<ThemeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILanguageContext _languageContext;

        public GetAllThemesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILanguageContext languageContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _languageContext = languageContext;
        }
        public async Task<IEnumerable<ThemeDTO>> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
        {
            //return _mapper.Map<IEnumerable<ThemeDTO>>(
            //        await _unitOfWork.GetRepository<ProductTheme, int>().GetAllAsync()
            //    );

            var spec = new ThemeWithTranslationsSpecification();
            var themes = await _unitOfWork.GetRepository<ProductTheme, int>().GetAllAsync(spec);
            var lang = _languageContext.CurrentLanguage;

            return themes.Select(t =>
            {
                var translation = TranslationHelper.GetTranslation(t, lang);
                return new ThemeDTO(t.Id, translation?.Name ?? "");
            });
        }
    }
}
