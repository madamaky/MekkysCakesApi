using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Commands.DeleteDeliveryMethod
{
    public class DeleteDeliveryMethodCommandHandler : IRequestHandler<DeleteDeliveryMethodCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDeliveryMethodCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(request.Id);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"The Delivery Method With Id {request.Id} Was Not Found");

            _unitOfWork.GetRepository<DeliveryMethod, int>().Delete(deliveryMethod);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
