using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using MekkysCakes.Services.Abstraction;

namespace MekkysCakes.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderToReturnDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<OrderToReturnDTO>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var email = _currentUserService.Email!;
            var spec = new OrderSpecification(request.OrderId, email);

            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"No Orders Were Found For User With Email {email}.");

            return _mapper.Map<OrderToReturnDTO>(order);
        }
    }
}