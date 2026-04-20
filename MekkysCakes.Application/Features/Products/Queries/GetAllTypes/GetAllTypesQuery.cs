using MediatR;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllTypes
{
    public record GetAllTypesQuery() : IRequest<IEnumerable<TypeDTO>>;
}
