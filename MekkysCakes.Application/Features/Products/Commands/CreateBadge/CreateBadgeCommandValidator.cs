using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.CreateBadge
{
    public class CreateBadgeCommandValidator : AbstractValidator<CreateBadgeCommand>
    {
        public CreateBadgeCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Badge name is required");
            RuleFor(x => x.Name.En).NotEmpty().MaximumLength(100).WithMessage("English badge name is required");
            RuleFor(x => x.Name.Ar).NotEmpty().MaximumLength(100).WithMessage("Arabic badge name is required");
        }
    }
}
