using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Services.Abstraction
{
    public interface IBasketService
    {
        Task<BasketDTO> GetBasketAsync(string basketId);
        Task<BasketDTO> CreateOrUpdateBasketAsync(string email, BasketDTO basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
