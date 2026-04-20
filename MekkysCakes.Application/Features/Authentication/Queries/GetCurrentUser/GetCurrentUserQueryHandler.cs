using MediatR;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDTO>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public GetCurrentUserQueryHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user is null)
                return Error.NotFound("User.NotFound", $"User With Email {request.Email} Was Not Found");

            var roles = await _identityService.GetRolesAsync(user);
            var token = await _tokenService.GenerateTokenAsync(user, roles);
            return new UserDTO(user.Email!, user.DisplayName, token);
        }
    }
}
