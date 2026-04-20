using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<UserDTO>>;
}
