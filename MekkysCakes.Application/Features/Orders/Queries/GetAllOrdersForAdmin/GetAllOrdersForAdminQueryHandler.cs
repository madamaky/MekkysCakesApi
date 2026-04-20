using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Application.Specifications.OrderSpecifications;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin
{
    public class GetAllOrdersForAdminQueryHandler : IRequestHandler<GetAllOrdersForAdminQuery, PaginatedResult<OrderToReturnDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersForAdminQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<OrderToReturnDTO>> Handle(GetAllOrdersForAdminQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Order, Guid>();
            var spec = new OrderSpecification(request.QueryParams);
            var orders = await repo.GetAllAsync(spec);

            var ordersToReturn = _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders);
            var totalOrdersCount = await repo.CountAsync(new OrdersCountSpecification(request.QueryParams));

            return new PaginatedResult<OrderToReturnDTO>
            (
                request.QueryParams.PageIndex,
                ordersToReturn.Count(),
                totalOrdersCount,
                ordersToReturn
            );
        }
    }
}
