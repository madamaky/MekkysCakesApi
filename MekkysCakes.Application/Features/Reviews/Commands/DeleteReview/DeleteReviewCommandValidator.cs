using FluentValidation;

namespace MekkysCakes.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0)
                .WithMessage("Review ID must be a positive number");

            RuleFor(x => x.UserEmail)
                .NotEmpty()
                .WithMessage("User email is required");
        }
    }
}
