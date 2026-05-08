using AutoMapper;
using MediatR;
using MekkysCakes.Application.Features.Products.Queries.GetAllThemes;
using MekkysCakes.Application.Helpers;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;


namespace MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods
{
    public class GetDeliveryMethodsQueryHandler : IRequestHandler<GetDeliveryMethodsQuery, Result<IEnumerable<DeliveryMethodDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILanguageContext _languageContext;

        public GetDeliveryMethodsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILanguageContext languageContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _languageContext = languageContext;
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            //var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            //if (!deliveryMethods.Any())
            //    return Error.NotFound("DeliveryMethod.NotFound", "No Delivery Methods Were Found.");

            //var data = _mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            //return Result<IEnumerable<DeliveryMethodDTO>>.Ok(data);

            var spec = new DeliveryMethodsWithTranslationsSpecification();
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(spec);
            if (!deliveryMethods.Any())
                return Error.NotFound("DeliveryMethod.NotFound", "No Delivery Methods Were Found.");
            var lang = _languageContext.CurrentLanguage;

            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(deliveryMethods.Select(t =>
            {
                var translation = TranslationHelper.GetTranslation(t, lang);
                return new DeliveryMethodDTO(t.Id, translation?.Name ?? "", t.Translations?.FirstOrDefault()?.Description ?? "", t.Translations?.FirstOrDefault()?.DeliveryTime ?? "", t.Price);
            }));
        }
    }
}
