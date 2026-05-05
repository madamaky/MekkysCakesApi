using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.CreateBadge
{
    public record CreateBadgeCommand(string Name) : IRequest<Result<bool>>;
}
