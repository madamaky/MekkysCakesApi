using AutoMapper;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.WishlistModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Services.Specifications.WishlistSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.WishlistDTOs;

namespace MekkysCakes.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WishlistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<WishlistItemDTO>>> GetWishlistAsync(string userEmail)
        {
            var spec = new WishlistWithItemsAndProductsSpecification(userEmail);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, Guid>().GetByIdAsync(spec);
            if (wishlist is null)
                return Result<IEnumerable<WishlistItemDTO>>.Ok([]);

            var wishlistItem = _mapper.Map<IEnumerable<WishlistItemDTO>>(wishlist.Items);
            return Result<IEnumerable<WishlistItemDTO>>.Ok(wishlistItem);
        }

        public async Task<Result<bool>> AddItemToWishlistAsync(string userEmail, int productId)
        {
            var productExists = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(productId);
            if (productExists is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {productId} Was Not Found.");

            var spec = new WishlistByEmailSpecification(userEmail);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, Guid>().GetByIdAsync(spec);
            if (wishlist is null)
            {
                wishlist = new Wishlist
                {
                    UserEmail = userEmail,
                    Items = new List<WishlistItem>
                    {
                        new WishlistItem { ProductId = productId }
                    }
                };
                await _unitOfWork.GetRepository<Wishlist, Guid>().AddAsync(wishlist);
                var result = await _unitOfWork.SaveChangesAsync();
                return result ? true : Error.Failure("Wishlist.Failure", $"Failed To Add Product With Id {productId} To The Wishlist");
            }

            if (wishlist.Items.Any(i => i.ProductId == productId))
                return Error.Validation("Wishlist.Validation", $"Product With Id {productId} Is Already In The Wishlist.");

            wishlist.Items.Add(new WishlistItem { ProductId = productId });

            var savedResult = await _unitOfWork.SaveChangesAsync();
            return savedResult ? true : Error.Failure("Wishlist.Failure", $"Failed To Add Product With Id {productId} To The Wishlist");
        }

        public async Task<Result<bool>> RemoveItemFromWishlistAsync(string userEmail, int productId)
        {
            var spec = new WishlistWithItemsSpecification(userEmail);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, Guid>().GetByIdAsync(spec);
            if (wishlist is null)
                return Error.NotFound("Wishlist.NotFound", $"Wishlist For The User With Email {userEmail} Was Not Found.");

            var itemToRemove = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {productId} Was Not Found In The Wishlist.");

            wishlist.Items.Remove(itemToRemove);
            var result = await _unitOfWork.SaveChangesAsync();
            return result ? true : Error.Failure("Wishlist.Failure", $"Failed to remove Product With Id {productId} From The Wishlist.");
        }
    }
}
