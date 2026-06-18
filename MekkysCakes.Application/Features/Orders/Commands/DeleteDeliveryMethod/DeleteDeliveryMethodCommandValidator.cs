using FluentValidation;

namespace MekkysCakes.Application.Features.Orders.Commands.DeleteDeliveryMethod
{
    public class DeleteDeliveryMethodCommandValidator : AbstractValidator<DeleteDeliveryMethodCommand>
    {
        public DeleteDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Delivery method Id must be a positive integer");
        }
    }
}
