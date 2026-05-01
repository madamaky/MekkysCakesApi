using FluentValidation;

namespace MekkysCakes.Application.Features.Wishlists.Commands.RemoveItemFromWishlist
{
    public class RemoveItemFromWishlistCommandValidator : AbstractValidator<RemoveItemFromWishlistCommand>
    {
        public RemoveItemFromWishlistCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product Id must be a positive integer");
        }
    }
}
