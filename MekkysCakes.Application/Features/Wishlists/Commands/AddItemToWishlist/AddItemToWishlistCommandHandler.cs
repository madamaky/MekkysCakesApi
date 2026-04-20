using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.WishlistModule;
using MekkysCakes.Application.Specifications.WishlistSpecifications;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Wishlists.Commands.AddItemToWishlist
{
    public class AddItemToWishlistCommandHandler : IRequestHandler<AddItemToWishlistCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddItemToWishlistCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(AddItemToWishlistCommand request, CancellationToken cancellationToken)
        {
            var productExists = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(request.ProductId);
            if (productExists is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {request.ProductId} Was Not Found.");

            var spec = new WishlistByEmailSpecification(request.UserEmail);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, Guid>().GetByIdAsync(spec);
            if (wishlist is null)
            {
                wishlist = new Wishlist
                {
                    UserEmail = request.UserEmail,
                    Items = new List<WishlistItem>
                    {
                        new WishlistItem { ProductId = request.ProductId }
                    }
                };
                await _unitOfWork.GetRepository<Wishlist, Guid>().AddAsync(wishlist);
                var result = await _unitOfWork.SaveChangesAsync();
                return result ? true : Error.Failure("Wishlist.Failure", $"Failed To Add Product With Id {request.ProductId} To The Wishlist");
            }

            if (wishlist.Items.Any(i => i.ProductId == request.ProductId))
                return Error.Validation("Wishlist.Validation", $"Product With Id {request.ProductId} Is Already In The Wishlist.");

            wishlist.Items.Add(new WishlistItem { ProductId = request.ProductId });

            var savedResult = await _unitOfWork.SaveChangesAsync();
            return savedResult ? true : Error.Failure("Wishlist.Failure", $"Failed To Add Product With Id {request.ProductId} To The Wishlist");
        }
    }
}
