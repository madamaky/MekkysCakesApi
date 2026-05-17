using MediatR;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllThemes
{
    public record GetAllThemesQuery() : IRequest<IEnumerable<ThemeDTO>>;

    public record ThemeDTO
    {
        public int Id { get; init; }
        public LocalizedString Name { get; init; } = new();
    }
}
