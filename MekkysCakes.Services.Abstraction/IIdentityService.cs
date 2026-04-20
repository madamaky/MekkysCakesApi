using MekkysCakes.Domain.Entities.IdentityModule;

namespace MekkysCakes.Services.Abstraction
{
    public interface IIdentityService
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<(bool Succeeded, IEnumerable<(string Code, string Description)> Errors)> CreateUserAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
    }
}
