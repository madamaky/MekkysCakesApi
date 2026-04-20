using MediatR;
using MekkysCakes.Domain.Entities.IdentityModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDTO>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                DisplayName = request.DisplayName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };

            var (succeeded, errors) = await _identityService.CreateUserAsync(user, request.Password);
            if (!succeeded)
                return errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            var roles = await _identityService.GetRolesAsync(user);
            var token = await _tokenService.GenerateTokenAsync(user, roles);
            return new UserDTO(user.Email, user.DisplayName, token);
        }
    }
}
