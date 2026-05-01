using MediatR;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllTypes
{
    public record GetAllTypesQuery() : IRequest<IEnumerable<TypeDTO>>;

    public record TypeDTO(int Id, string Name);
}
