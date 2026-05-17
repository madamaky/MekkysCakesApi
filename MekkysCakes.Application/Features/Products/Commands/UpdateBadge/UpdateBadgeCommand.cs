using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateBadge
{
    public record UpdateBadgeCommand(int Id, LocalizedString Name) : IRequest<Result<bool>>;
}
