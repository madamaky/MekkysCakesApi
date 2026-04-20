using FluentValidation;

namespace MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket
{
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.BasketId)
                .NotEmpty().WithMessage("Basket Id is required");
        }
    }
}
