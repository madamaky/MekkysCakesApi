using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.WishlistDTOs;

namespace MekkysCakes.Services.Abstraction
{
    public interface IWishlistService
    {
        Task<Result<IEnumerable<WishlistItemDTO>>> GetWishlistAsync(string userEmail);
        Task<Result<bool>> AddItemToWishlistAsync(string userEmail, int productId);
        Task<Result<bool>> RemoveItemFromWishlistAsync(string userEmail, int productId);
    }
}
