using IdentityServer.Models;
using IdentityServer.Records;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Database
{
    public class AuthDataSeeder(UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        IOpenIddictScopeManager scopesManager,
        IOpenIddictApplicationManager iddictManager)
    {
        public async Task SeedAllAsync()
        {
            await SeedRoles();
            await SeedUsers();
            await CreateScopesAsync();
            await CreateClientApplicationsAsync();
        }

        private async Task SeedRoles()
        {
            if (!await roleManager.RoleExistsAsync(ApplicationRole.SuperAdmin.Name))
                await roleManager.CreateAsync(ApplicationRole.SuperAdmin);

            if (!await roleManager.RoleExistsAsync(ApplicationRole.TenantAdmin.Name))
                await roleManager.CreateAsync(ApplicationRole.TenantAdmin);

            if (!await roleManager.RoleExistsAsync(ApplicationRole.TenantUser.Name))
                await roleManager.CreateAsync(ApplicationRole.TenantUser);
        }

        private async Task SeedUsers()
        {
            if (await userManager.FindByEmailAsync("superadmin@yourapp.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "superadmin@yourapp.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "superadmin@yourapp.com",
                };

                var result = await userManager.CreateAsync(user, "Test@123");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, ApplicationRole.SuperAdmin.Name);
            }
        }

        private async Task CreateScopesAsync()
        {
            List<ScopeRecord> scopes = new()
            {
                new ScopeRecord
                {
                    DisplayName = "auth",
                    Name = "auth-api",
                    Resources = "auth-api"
                },
                new ScopeRecord
                {
                    DisplayName = "billing",
                    Name = "billing-api",
                    Resources = "billing-api"
                },
                new ScopeRecord
                {
                    DisplayName = "product",
                    Name = "product-api",
                    Resources = "product-api"
                },
                new ScopeRecord
                {
                    DisplayName = "subscription",
                    Name = "subscription-api",
                    Resources = "subscription-api"
                },
                new ScopeRecord
                {
                    DisplayName = "tenant",
                    Name = "tenant-api",
                    Resources = "tenant-api"
                }
            };

            foreach (var item in scopes)
            {

                var apiScope = await scopesManager.FindByNameAsync(item.Name);

                if (apiScope != null)
                {
                    await scopesManager.DeleteAsync(apiScope);
                }

                await scopesManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = item.DisplayName,
                    Name = item.Name,
                    Resources =
                    {
                        item.Resources
                    }
                });
            }
        }

        private async Task CreateClientApplicationsAsync()
        {
            var result = (OpenIddictEntityFrameworkCoreApplication)await iddictManager.FindByClientIdAsync("product-cloud");
            if (result != null)
            {
                return;
            }

            var clientSecret = Guid.NewGuid().ToString();
            var application = new OpenIddictApplicationDescriptor
            {
                ClientId = "product-cloud",
                ClientSecret = clientSecret,
                ApplicationType = ClientTypes.Public,
                ConsentType = ConsentTypes.Explicit,
                DisplayName = "Product Cloud",
                RedirectUris =
                {
                    new Uri("http://localhost:3000/auth/callback")
                },
                PostLogoutRedirectUris =
                {
                    new Uri("http://localhost:3000/auth/callback")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "product-api",
                    Permissions.Prefixes.Scope + "tenant-api",
                    Permissions.Prefixes.Scope + "billing-api",
                    Permissions.Prefixes.Scope + "subscription-api",
                    Permissions.Prefixes.Scope + "auth-api"
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
            };

            await iddictManager.CreateAsync(application);

            Console.WriteLine($"Client Secret: {clientSecret}");
        }
    }
}
