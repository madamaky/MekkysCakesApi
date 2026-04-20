using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Application.Features.Baskets.Queries.GetBasket
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<BasketDTO>>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public GetBasketQueryHandler(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", $"Basket With Id {request.BasketId} Was Not Found");

            return _mapper.Map<BasketDTO>(basket);
        }
    }
}
