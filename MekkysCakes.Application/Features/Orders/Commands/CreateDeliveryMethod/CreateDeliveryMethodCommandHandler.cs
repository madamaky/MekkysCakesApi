using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateDeliveryMethod
{
    public class CreateDeliveryMethodCommandHandler : IRequestHandler<CreateDeliveryMethodCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDeliveryMethodCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CreateDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethod = new DeliveryMethod
            {
                Price = request.Price,
                Translations =
                [
                    new() { Language = "en", Name = request.Name.En, Description = request.Description.En, DeliveryTime = request.DeliveryTime.En },
                    new() { Language = "ar", Name = request.Name.Ar, Description = request.Description.Ar, DeliveryTime = request.DeliveryTime.Ar }
                ]
            };

            await _unitOfWork.GetRepository<DeliveryMethod, int>().AddAsync(deliveryMethod);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
