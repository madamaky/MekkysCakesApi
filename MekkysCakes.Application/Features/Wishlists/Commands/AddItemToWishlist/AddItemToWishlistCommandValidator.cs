using FluentValidation;

namespace MekkysCakes.Application.Features.Wishlists.Commands.AddItemToWishlist
{
    public class AddItemToWishlistCommandValidator : AbstractValidator<AddItemToWishlistCommand>
    {
        public AddItemToWishlistCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product Id must be a positive integer");
        }
    }
}
