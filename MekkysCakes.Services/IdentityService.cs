using MekkysCakes.Domain.Entities.IdentityModule;
using MekkysCakes.Services.Abstraction;
using Microsoft.AspNetCore.Identity;

namespace MekkysCakes.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
            => await _userManager.CheckPasswordAsync(user, password);

        public async Task<(bool Succeeded, IEnumerable<(string Code, string Description)> Errors)> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            var errors = result.Errors.Select(e => (e.Code, e.Description));
            return (result.Succeeded, errors);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => await _userManager.GetRolesAsync(user);
    }
}
