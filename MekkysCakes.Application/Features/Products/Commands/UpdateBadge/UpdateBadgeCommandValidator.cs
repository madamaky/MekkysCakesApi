using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateBadge
{
    public class UpdateBadgeCommandValidator : AbstractValidator<UpdateBadgeCommand>
    {
        public UpdateBadgeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Badge Id must be a positive integer");

            RuleFor(x => x.Name).NotNull().WithMessage("Badge name is required");
            RuleFor(x => x.Name.En).NotEmpty().MaximumLength(100).WithMessage("English badge name is required");
            RuleFor(x => x.Name.Ar).NotEmpty().MaximumLength(100).WithMessage("Arabic badge name is required");
        }
    }
}
