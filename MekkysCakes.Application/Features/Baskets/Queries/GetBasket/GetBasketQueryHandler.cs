using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Application.Features.Baskets.Queries.GetBasket
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<BasketDTO>>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetBasketQueryHandler(IBasketRepository basketRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var userEmail = _currentUserService.Email;
            if (string.IsNullOrEmpty(userEmail))
                return Error.Unauthorized("User.Unauthorized", "You Must Be Logged In To View Your Basket");

            var basket = await _basketRepository.GetBasketAsync(userEmail);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", $"Basket For User: {userEmail} Was Not Found");

            return _mapper.Map<BasketDTO>(basket);
        }
    }
}
