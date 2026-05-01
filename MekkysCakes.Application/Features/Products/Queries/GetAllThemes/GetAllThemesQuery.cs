using MediatR;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllThemes
{
    public record GetAllThemesQuery() : IRequest<IEnumerable<ThemeDTO>>;

    public record ThemeDTO(int Id, string Name);
}
