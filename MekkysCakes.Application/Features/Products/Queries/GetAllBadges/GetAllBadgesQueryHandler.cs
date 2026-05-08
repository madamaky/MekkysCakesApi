using AutoMapper;
using MediatR;
using MekkysCakes.Application.Helpers;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllBadges
{
    public record GetAllBadgesQueryHandler : IRequestHandler<GetAllBadgesQuery, IEnumerable<BadgeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILanguageContext _languageContext;

        public GetAllBadgesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILanguageContext languageContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _languageContext = languageContext;
        }

        public async Task<IEnumerable<BadgeDTO>> Handle(GetAllBadgesQuery request, CancellationToken cancellationToken)
        {
            //return _mapper.Map<IEnumerable<BadgeDTO>>(
            //        await _unitOfWork.GetRepository<Badge, int>().GetAllAsync()
            //    );

            var spec = new BadgeWithTranslationsSpecification();
            var badges = await _unitOfWork.GetRepository<Badge, int>().GetAllAsync(spec);
            var lang = _languageContext.CurrentLanguage;

            return badges.Select(b =>
            {
                var t = TranslationHelper.GetTranslation(b, lang);
                return new BadgeDTO(b.Id, t?.Name ?? "");
            });
        }
    }
}
