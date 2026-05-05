using MediatR;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllBadges
{
    public record GetAllBadgesQuery : IRequest<IEnumerable<BadgeDTO>>;

    public record BadgeDTO(int Id, string Name);
}
