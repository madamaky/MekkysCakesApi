using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateBadge
{
    public record UpdateBadgeCommand(int Id, string Name) : IRequest<Result<bool>>;
}
