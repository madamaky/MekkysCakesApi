using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand
    (
        LocalizedString Name,
        LocalizedString Description,
        string PictureUrl,
        decimal Price,
        int TypeId,
        int ThemeId,
        List<int> BadgeIds
    ) : IRequest<Result<bool>>;
}
