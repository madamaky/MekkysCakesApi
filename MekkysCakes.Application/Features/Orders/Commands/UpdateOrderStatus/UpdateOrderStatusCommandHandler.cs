using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(request.OrderId);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"Order With Id {request.OrderId} Was Not Found.");

            if (order.OrderStatus == (OrderStatus)request.NewStatus)
                return Error.Validation("Order.Validation", $"Order With Id {request.OrderId} Is Already With Status {request.NewStatus}");

            order.OrderStatus = (OrderStatus)request.NewStatus;

            var result = await _unitOfWork.SaveChangesAsync();
            return result ? true : Error.Failure("Order.Failure", $"Failed To Update Status For Order With Id {request.OrderId}.");
        }
    }
}
