using MekkysCakes.Domain;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MekkysCakes.Persistence.Data.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.TopTierHuman));
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.SuperAdmin));
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
                }

                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser
                    {
                        DisplayName = "MADA",
                        UserName = "mada",
                        Email = "mada@gmail.com",
                        PhoneNumber = "1234567890"
                    };
                    var user02 = new ApplicationUser
                    {
                        DisplayName = "SAEED",
                        UserName = "saeed",
                        Email = "saeed@gmail.com",
                        PhoneNumber = "1234567891"
                    };

                    await _userManager.CreateAsync(user01, "P@$$w0rd");
                    await _userManager.CreateAsync(user02, "P@$$w0rd");

                    await _userManager.AddToRoleAsync(user01, AppRoles.TopTierHuman);
                    await _userManager.AddToRoleAsync(user02, AppRoles.SuperAdmin);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during identity data initialization: {ex.Message}");
            }
        }
    }
}

