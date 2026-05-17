using FluentValidation;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product Id must be a positive integer");

            RuleFor(x => x.Name).NotNull().WithMessage("Product name is required");
            RuleFor(x => x.Name.En).NotEmpty().MaximumLength(100).WithMessage("English product name is required");
            RuleFor(x => x.Name.Ar).NotEmpty().MaximumLength(100).WithMessage("Arabic product name is required");

            RuleFor(x => x.Description).NotNull().WithMessage("Product description is required");
            RuleFor(x => x.Description.En).NotEmpty().MaximumLength(500).WithMessage("English product description is required");
            RuleFor(x => x.Description.Ar).NotEmpty().MaximumLength(500).WithMessage("Arabic product description is required");

            RuleFor(x => x.PictureUrl)
                .NotEmpty().WithMessage("Product picture URL is required");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.TypeId)
                .GreaterThan(0).WithMessage("Type Id must be a positive integer");

            RuleFor(x => x.ThemeId)
                .GreaterThan(0).WithMessage("Theme Id must be a positive integer");

            RuleFor(x => x.BadgeIds)
                .NotNull().WithMessage("Badge IDs list is required");

            RuleForEach(x => x.BadgeIds)
                .GreaterThan(0).WithMessage("Each Badge Id must be a positive integer");
        }
    }
}
