using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Commands.CreateBadge
{
    public record CreateBadgeCommand(LocalizedString Name) : IRequest<Result<bool>>;
}
