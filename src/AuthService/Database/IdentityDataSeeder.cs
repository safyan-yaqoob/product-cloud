using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Database
{
    public class IdentityDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        public async Task SeedAllAsync()
        {
            await SeedRoles();
            await SeedUsers();
        }

        private async Task SeedRoles()
        {
            if (!await roleManager.RoleExistsAsync(ApplicationRole.Admin.Name))
                await roleManager.CreateAsync(ApplicationRole.Admin);

            if (!await roleManager.RoleExistsAsync(ApplicationRole.User.Name))
                await roleManager.CreateAsync(ApplicationRole.User);
        }

        private async Task SeedUsers()
        {
            if (await userManager.FindByEmailAsync("test@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "test",
                    //FirstName = "test",
                    //LastName = "test",
                    Email = "test@test.com",
                };

                var result = await userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, ApplicationRole.Admin.Name);
            }

            if (await userManager.FindByEmailAsync("test2@test.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "test2",
                    //FirstName = "test",
                    //LastName = "test",
                    Email = "test2@test.com"
                };

                var result = await userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, ApplicationRole.User.Name);
            }
        }
    }
}
