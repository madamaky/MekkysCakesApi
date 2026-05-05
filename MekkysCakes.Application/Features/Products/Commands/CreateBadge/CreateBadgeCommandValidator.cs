using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.CreateBadge
{
    public class CreateBadgeCommandValidator : AbstractValidator<CreateBadgeCommand>
    {
        public CreateBadgeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Badge name is required")
                .MaximumLength(50).WithMessage("Badge name must not exceed 50 characters");
        }
    }
}
