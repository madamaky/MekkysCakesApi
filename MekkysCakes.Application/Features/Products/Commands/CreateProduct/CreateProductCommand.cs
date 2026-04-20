using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        string PictureUrl,
        decimal Price,
        int TypeId,
        int ThemeId
    ) : IRequest<Result<bool>>;
}
