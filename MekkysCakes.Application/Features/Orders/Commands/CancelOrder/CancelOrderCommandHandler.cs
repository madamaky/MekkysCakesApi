using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(request.OrderId);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"Order With Id {request.OrderId} Was Not Found.");

            if (order.UserEmail != request.Email)
                return Error.Validation("Order.Validation", $"User With Email {request.Email} Does Not Have Order With Id {request.OrderId}.");

            if (order.OrderStatus == OrderStatus.Cancelled)
                return Error.Validation("Order.Validation", $"Order With Id {request.OrderId} Was Already Cancelled.");

            if (order.OrderStatus != OrderStatus.Pending)
                return Error.Validation("Order.Validation", $"Only Pending Orders Can Be Cancelled. Order With Id {request.OrderId} Is With Status {order.OrderStatus}.");

            order.OrderStatus = OrderStatus.Cancelled;

            var result = await _unitOfWork.SaveChangesAsync();
            return result ? true : Error.Failure("Order.Failure", $"Failed To Update Status For Order With Id {request.OrderId}.");
        }
    }
}
