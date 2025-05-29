using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace IdentityServer.Models
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public static ApplicationRole TenantUser => new()
        {
            Name = Constants.Role.TenantUser,
            NormalizedName = nameof(TenantUser).ToUpper(CultureInfo.InvariantCulture),
        };

        public static ApplicationRole TenantAdmin => new()
        {
            Name = Constants.Role.TenantAdmin,
            NormalizedName = nameof(TenantAdmin).ToUpper(CultureInfo.InvariantCulture)
        };
        public static ApplicationRole SuperAdmin => new()
        {
            Name = Constants.Role.SuperAdmin,
            NormalizedName = nameof(SuperAdmin).ToUpper(CultureInfo.InvariantCulture)
        };
    }

    public static class Constants
    {
        public static class Role
        {
            public const string SuperAdmin = "super-admin";
            public const string TenantAdmin = "tenant-admin";
            public const string TenantUser = "tenant-user";
        }

        public static string IdentityRoleName => "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    }
}
