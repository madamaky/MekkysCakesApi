using FluentValidation;

namespace MekkysCakes.Application.Features.Reviews.Commands.ApproveReview
{
    public class ApproveReviewCommandValidator : AbstractValidator<ApproveReviewCommand>
    {
        public ApproveReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0)
                .WithMessage("Review ID must be a positive number");
        }
    }
}
