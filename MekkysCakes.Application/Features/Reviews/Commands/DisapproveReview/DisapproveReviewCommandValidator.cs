using FluentValidation;

namespace MekkysCakes.Application.Features.Reviews.Commands.DisapproveReview
{
    public class DisapproveReviewCommandValidator : AbstractValidator<DisapproveReviewCommand>
    {
        public DisapproveReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0)
                .WithMessage("Review ID must be a positive number");
        }
    }
}
