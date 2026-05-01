using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using MekkysCakes.Services.Abstraction;

namespace MekkysCakes.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<IEnumerable<OrderToReturnDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var email = _currentUserService.Email!;
            var spec = new OrderSpecification(email);

            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            if (orders is null || !orders.Any())
                return Error.NotFound("Order.NotFound", $"No Orders Were Found For User With Email {email}.");

            var data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(data);
        }
    }
}
