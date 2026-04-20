using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery(string Email) : IRequest<Result<UserDTO>>;
}
