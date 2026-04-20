using FluentValidation;

namespace MekkysCakes.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order Id is required");
        }
    }
}
