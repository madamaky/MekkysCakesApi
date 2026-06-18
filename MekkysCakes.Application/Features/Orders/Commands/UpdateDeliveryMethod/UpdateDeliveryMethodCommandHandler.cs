using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Commands.UpdateDeliveryMethod
{
    public class UpdateDeliveryMethodCommandHandler : IRequestHandler<UpdateDeliveryMethodCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDeliveryMethodCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(request.Id);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"The Delivery Method With Id {request.Id} Was Not Found");

            deliveryMethod.Price = request.Price;
            deliveryMethod.Translations =
            [
                new() { Language = "en", Name = request.Name.En, Description = request.Description.En, DeliveryTime = request.DeliveryTime.En },
                new() { Language = "ar", Name = request.Name.Ar, Description = request.Description.Ar, DeliveryTime = request.DeliveryTime.Ar }
            ];

            _unitOfWork.GetRepository<DeliveryMethod, int>().Update(deliveryMethod);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
