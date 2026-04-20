using MediatR;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllThemes
{
    public record GetAllThemesQuery() : IRequest<IEnumerable<ThemeDTO>>;
}
