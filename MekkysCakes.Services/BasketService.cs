using AutoMapper;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.BasketModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Services.Exceptions;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<BasketDTO> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                throw new BasketNotFoundException(basketId);

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<BasketDTO> CreateOrUpdateBasketAsync(string email, BasketDTO basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            customerBasket.Id = email;
            var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            return _mapper.Map<BasketDTO>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId) => await _basketRepository.DeleteBasketAsync(basketId);
    }
}
