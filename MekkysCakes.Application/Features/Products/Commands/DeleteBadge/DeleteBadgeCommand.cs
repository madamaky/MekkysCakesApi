using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.DeleteBadge
{
    public record DeleteBadgeCommand(int Id) : IRequest<Result<bool>>;
}
