using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.DeleteBadge
{
    public class DeleteBadgeCommandValidator : AbstractValidator<DeleteBadgeCommand>
    {
        public DeleteBadgeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Badge Id must be a positive integer");
        }
    }
}
