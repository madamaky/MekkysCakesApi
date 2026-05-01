using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.WishlistModule;
using MekkysCakes.Application.Specifications.WishlistSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Services.Abstraction;

namespace MekkysCakes.Application.Features.Wishlists.Commands.RemoveItemFromWishlist
{
    public class RemoveItemFromWishlistCommandHandler : IRequestHandler<RemoveItemFromWishlistCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveItemFromWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(RemoveItemFromWishlistCommand request, CancellationToken cancellationToken)
        {
            var email = _currentUserService.Email!;
            var spec = new WishlistWithItemsSpecification(email);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, Guid>().GetByIdAsync(spec);
            if (wishlist is null)
                return Error.NotFound("Wishlist.NotFound", $"Wishlist For The User With Email {email} Was Not Found.");
            var itemToRemove = wishlist.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (itemToRemove is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {request.ProductId} Was Not Found In The Wishlist.");

            wishlist.Items.Remove(itemToRemove);
            var result = await _unitOfWork.SaveChangesAsync();
            return result ? true : Error.Failure("Wishlist.Failure", $"Failed to remove Product With Id {request.ProductId} From The Wishlist.");
        }
    }
}
