using FluentValidation;

namespace MekkysCakes.Application.Features.Baskets.Commands.CreateOrUpdateBasket
{
    public class CreateOrUpdateBasketCommandValidator : AbstractValidator<CreateOrUpdateBasketCommand>
    {
        public CreateOrUpdateBasketCommandValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items collection is required")
                .Must(items => items != null && items.Count > 0).WithMessage("Basket must contain at least one item");
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.Id)
                    .GreaterThan(0).WithMessage("Item Id must be a positive integer");
                item.RuleFor(i => i.ProductName)
                    .NotEmpty().WithMessage("Product name is required");
                item.RuleFor(i => i.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than 0");
                item.RuleFor(i => i.Quantity)
                    .InclusiveBetween(1, 100).WithMessage("Quantity must be between 1 and 100");
            });
        }
    }
}
