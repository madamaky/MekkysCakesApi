using MediatR;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllTypes
{
    public record GetAllTypesQuery() : IRequest<IEnumerable<TypeDTO>>;

    public record TypeDTO
    {
        public int Id { get; init; }
        public LocalizedString Name { get; init; } = new();
    }
}
