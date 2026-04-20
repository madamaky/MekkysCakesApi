using FluentValidation;

namespace MekkysCakes.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order Id is required");
            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("Invalid order status value");
        }
    }
}
