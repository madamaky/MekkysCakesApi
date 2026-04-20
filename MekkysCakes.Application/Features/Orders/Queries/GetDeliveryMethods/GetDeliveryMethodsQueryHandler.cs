using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods
{
    public class GetDeliveryMethodsQueryHandler : IRequestHandler<GetDeliveryMethodsQuery, Result<IEnumerable<DeliveryMethodDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDeliveryMethodsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!deliveryMethods.Any())
                return Error.NotFound("DeliveryMethod.NotFound", "No Delivery Methods Were Found.");

            var data = _mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(data);
        }
    }
}
