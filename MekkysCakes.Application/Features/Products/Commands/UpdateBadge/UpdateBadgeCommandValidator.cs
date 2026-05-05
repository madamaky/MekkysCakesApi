using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateBadge
{
    public class UpdateBadgeCommandValidator : AbstractValidator<UpdateBadgeCommand>
    {
        public UpdateBadgeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Badge Id must be a positive integer");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Badge name is required")
                .MaximumLength(50).WithMessage("Badge name must not exceed 50 characters");
        }
    }
}
