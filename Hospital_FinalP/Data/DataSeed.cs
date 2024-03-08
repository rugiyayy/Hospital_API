using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hospital_FinalP.Data
{
    public class DataSeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            #region Roles

            var roles = new string[] { "Admin", "Scheduler","Doctor", "Patient" };
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in roles)
            {
                var existingRole = await roleManager.FindByNameAsync(role);
                if (existingRole != null) continue;
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            #endregion

            #region Admin Creation

            string adminUserName = (string)configuration["DefaultAdmin:UserName"]!;
            string adminPassword = (string)configuration["DefaultAdmin:Password"]!;

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var admin = await userManager.FindByNameAsync(adminUserName);

            if (admin != null) return;

            admin = new AppUser
            {
               FullName = "AdminsFullName",
                UserName = "adminA",
                Email = "adminA@gmail.com",
            };

            await userManager.CreateAsync(admin, adminPassword);

            await userManager.AddToRoleAsync(admin, roles[0]);
            #endregion
        }

    }
}
