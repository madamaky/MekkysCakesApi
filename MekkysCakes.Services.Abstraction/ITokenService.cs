using MekkysCakes.Domain.Entities.IdentityModule;

namespace MekkysCakes.Services.Abstraction
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles);
    }
}
