using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Result<bool>>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteBasketCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var result = await _basketRepository.DeleteBasketAsync(request.BasketId);
            return result ? true : Error.NotFound("Basket.NotFound", $"Basket With Id {request.BasketId} Was Not Found");
        }
    }
}
