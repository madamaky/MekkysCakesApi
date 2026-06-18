using FluentValidation;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateDeliveryMethod
{
    public class CreateDeliveryMethodCommandValidator : AbstractValidator<CreateDeliveryMethodCommand>
    {
        public CreateDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Delivery method name is required");
            RuleFor(x => x.Name.En).NotEmpty().MaximumLength(100).WithMessage("English delivery method name is required");
            RuleFor(x => x.Name.Ar).NotEmpty().MaximumLength(100).WithMessage("Arabic delivery method name is required");

            RuleFor(x => x.Description).NotNull().WithMessage("Delivery method description is required");
            RuleFor(x => x.Description.En).NotEmpty().MaximumLength(500).WithMessage("English delivery method description is required");
            RuleFor(x => x.Description.Ar).NotEmpty().MaximumLength(500).WithMessage("Arabic delivery method description is required");

            RuleFor(x => x.DeliveryTime).NotNull().WithMessage("Delivery time is required");
            RuleFor(x => x.DeliveryTime.En).NotEmpty().MaximumLength(100).WithMessage("English delivery time is required");
            RuleFor(x => x.DeliveryTime.Ar).NotEmpty().MaximumLength(100).WithMessage("Arabic delivery time is required");

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value");
        }
    }
}
