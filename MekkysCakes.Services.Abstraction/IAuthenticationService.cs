using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.IdentityDTOs;

namespace MekkysCakes.Services.Abstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    }
}
