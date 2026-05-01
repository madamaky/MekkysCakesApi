using MediatR;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDTO>>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserQueryHandler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<UserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var email = _currentUserService.Email!;
            var user = await _identityService.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound("User.NotFound", $"User With Email {email} Was Not Found");

            var roles = await _identityService.GetRolesAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, _currentUserService.Token!);
        }
    }
}