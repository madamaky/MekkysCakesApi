using AutoMapper;
using MediatR;
using MekkysCakes.Application.Specifications.ProductSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllThemes
{
    public class GetAllThemesQueryHandler : IRequestHandler<GetAllThemesQuery, IEnumerable<ThemeDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllThemesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ThemeDTO>> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ThemeWithTranslationsSpecification();
            var themes = await _unitOfWork.GetRepository<ProductTheme, int>().GetAllAsync(spec);

            return _mapper.Map<IEnumerable<ThemeDTO>>(themes);
        }
    }
}
