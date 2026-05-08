using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.CreateBadge
{
    public class CreateBadgeCommandValidator : AbstractValidator<CreateBadgeCommand>
    {
        public CreateBadgeCommandValidator()
        {
            //RuleFor(x => x.Name)
            //    .NotEmpty().WithMessage("Badge name is required")
            //    .MaximumLength(50).WithMessage("Badge name must not exceed 50 characters");

            RuleFor(x => x.Translations)
                .NotEmpty().WithMessage("At least one translation is required")
                .Must(translations => translations.Select(t => t.Language).Distinct().Count() == translations.Count)
                .WithMessage("Each translation must have a unique language");

            RuleForEach(x => x.Translations).ChildRules(t =>
            {
                t.RuleFor(x => x.Language).NotEmpty().Must(l => l == "en" || l == "ar");
                t.RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            });
            // Ensure English translation exists
            RuleFor(x => x.Translations)
                .Must(t => t.Any(tr => tr.Language == "en"))
                .WithMessage("English translation is required");
        }
    }
}
