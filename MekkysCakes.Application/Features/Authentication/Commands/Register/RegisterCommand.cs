using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string DisplayName,
        string UserName,
        string Password,
        string PhoneNumber
    ) : IRequest<Result<UserDTO>>;
}
