using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderToReturnDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderToReturnDTO>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new OrderSpecification(request.OrderId, request.Email);

            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"No Orders Were Found For User With Email {request.Email}.");

            return _mapper.Map<OrderToReturnDTO>(order);
        }
    }
}
