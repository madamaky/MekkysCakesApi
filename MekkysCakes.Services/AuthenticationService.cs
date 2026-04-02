using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MekkysCakes.Domain.Entities.IdentityModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MekkysCakes.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.email);
            if (user is null)
                return Error.InvalidCredentials("User.InvalidCredentials", "Email Does Not Exist");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.password);
            if (!isPasswordValid)
                return Error.InvalidCredentials("User.InvalidCredentials", "Wrong Password");

            var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                Email = registerDTO.email,
                DisplayName = registerDTO.displayName,
                UserName = registerDTO.userName,
                PhoneNumber = registerDTO.phoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerDTO.password);
            if (!result.Succeeded)
                return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email, user.DisplayName, token);
        }

        public async Task<bool> CheckEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email) is not null;

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound("User.NotFound", $"User With Email {email} Was Not Found");

            return new UserDTO(user.Email!, user.DisplayName, await CreateTokenAsync(user));
        }



        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTOptions:SecretKey"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(50),
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
