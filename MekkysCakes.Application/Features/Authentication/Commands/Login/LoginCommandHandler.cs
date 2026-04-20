using MediatR;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserDTO>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user is null)
                return Error.InvalidCredentials("User.InvalidCredentials", "Email Does Not Exist");

            var isPasswordValid = await _identityService.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return Error.InvalidCredentials("User.InvalidCredentials", "Wrong Password");

            var roles = await _identityService.GetRolesAsync(user);
            var token = await _tokenService.GenerateTokenAsync(user, roles);
            return new UserDTO(user.Email!, user.DisplayName, token);
        }
    }
}
