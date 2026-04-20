using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<IEnumerable<OrderToReturnDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var spec = new OrderSpecification(request.Email);

            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            if (orders is null || !orders.Any())
                return Error.NotFound("Order.NotFound", $"No Orders Were Found For User With Email {request.Email}.");

            var data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(data);
        }
    }
}
