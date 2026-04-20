using FluentValidation;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");
            RuleFor(x => x.BasketId)
                .NotEmpty().WithMessage("Basket Id is required");
            RuleFor(x => x.DeliveryMethodId)
                .GreaterThan(0).WithMessage("Delivery Method Id must be a positive integer");
            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required");
            RuleFor(x => x.Address.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .When(x => x.Address is not null);
            RuleFor(x => x.Address.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .When(x => x.Address is not null);
            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("Street is required")
                .When(x => x.Address is not null);
            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("City is required")
                .When(x => x.Address is not null);
        }
    }
}
