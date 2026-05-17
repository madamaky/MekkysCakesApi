using MediatR;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllBadges
{
    public record GetAllBadgesQuery : IRequest<IEnumerable<BadgeDTO>>;

    public record BadgeDTO
    {
        public int Id { get; init; }
        public LocalizedString Name { get; init; } = new();
    }
}
