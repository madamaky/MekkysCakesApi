using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.BasketModule;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Application.Features.Baskets.Commands.CreateOrUpdateBasket
{
    public class CreateOrUpdateBasketCommandHandler : IRequestHandler<CreateOrUpdateBasketCommand, Result<BasketDTO>>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public CreateOrUpdateBasketCommandHandler(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> Handle(CreateOrUpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var basketDto = new BasketDTO(request.Items);
            var customerBasket = _mapper.Map<CustomerBasket>(basketDto);
            customerBasket.Id = request.Email;

            var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            if (createdOrUpdatedBasket is null)
                return Error.Failure("Basket.Failure", "Failed to create or update the basket");

            return _mapper.Map<BasketDTO>(createdOrUpdatedBasket);
        }
    }
}
