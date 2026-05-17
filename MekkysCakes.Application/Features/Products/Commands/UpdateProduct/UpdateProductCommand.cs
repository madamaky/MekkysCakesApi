using System.Text.Json.Serialization;
using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        [property: JsonIgnore] int Id,
        LocalizedString Name,
        LocalizedString Description,
        string PictureUrl,
        decimal Price,
        int TypeId,
        int ThemeId,
        List<int> BadgeIds
    ) : IRequest<Result<bool>>;
}
