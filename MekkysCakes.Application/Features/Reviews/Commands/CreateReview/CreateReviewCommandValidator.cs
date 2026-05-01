using FluentValidation;

namespace MekkysCakes.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be a positive number");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Title)
                .MaximumLength(150)
                .When(x => x.Title is not null)
                .WithMessage("Review title must not exceed 150 characters");

            RuleFor(x => x.Comment)
                .MaximumLength(2000)
                .When(x => x.Comment is not null)
                .WithMessage("Review comment must not exceed 2000 characters");
        }
    }
}
